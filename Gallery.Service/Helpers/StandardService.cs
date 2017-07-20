using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Service.Helpers
{
    //This prevents an issue with the component desinger that cannot handle abstract classes
    [System.ComponentModel.DesignerCategory("")]
    public abstract class StandardService : ServiceBase
    {
        const string LogFileNameExtension = ".log";

        readonly string logFilePath;
        readonly string logLevelMin;
        readonly Type type;
        readonly AssemblyName assemblyName;

        TraceSource Logger = new TraceSource(nameof(Service));

        public StandardService(string logFilePath, string logLevelMin)
        {
            this.logFilePath = logFilePath;
            this.logLevelMin = logLevelMin;
            type = this.GetType();
            assemblyName = type.Assembly.GetName();
        }

        public virtual void Run(string[] args)
        {
            string logFileName = Path.Combine(logFilePath, assemblyName.Name, LogFileNameExtension);

            TextWriterTraceListener textWriterTraceListener = new TextWriterTraceListener(logFileName);
            Logger.Listeners.Add(textWriterTraceListener);

            //Debugging the service in VS will also bring the ConsoleSwitch in the args list.
            //Since debug output in VS goes to the Output Window anyway there is no need to attach a console here.
            if (args.Contains(ServiceManager.ConsoleSwitch) && !Debugger.IsAttached)
            {
                //This is required as a service projects have per default no console window. -1 means attach to current console window.
                NativeMethods.AttachConsole(-1);

                ConsoleTraceListener consoleTraceListener = new ConsoleTraceListener();
                Logger.Listeners.Add(consoleTraceListener);
            }

            //We assume that all assemblies without public key token are our own assemblies.
            IEnumerable<AssemblyName> referencedAssemblies = type.Assembly.GetReferencedAssemblies().Where(x => x.GetPublicKeyToken().Length == 0);
            Logger.TraceInformation($"Service is starting. Executable version: {assemblyName.Version}. Referenced assemblies: {String.Join("; ", referencedAssemblies)}");
        }

        protected AssemblyName AssemblyName
        {
            get
            {
                return assemblyName;
            }
        }

        protected override void OnStart(string[] args)
        {
            Run(args);
        }

        protected override void OnStop()
        {
            Logger.TraceInformation($"Service is stopping. Executable version: {assemblyName.Version}.");
        }
    }
}
