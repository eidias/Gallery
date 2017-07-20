using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gallery.Service.Helpers
{
    public class ServiceManager
    {
        public const string ConsoleSwitch = "-Console";
        public const string LocalTraceSwitch = "-LocalTrace";
        public static readonly IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        public static string HostName => $"{ipGlobalProperties.HostName}.{ipGlobalProperties.DomainName}";


        public static void Run(StandardService service, string[] args)
        {
            //This prevents the default message saying you cannot start a ServiceProject when starting from the console
            //Need to supply the -Console switch as command line argument in the debug section of the project properties
            if (args.Contains(ServiceManager.ConsoleSwitch))
            {
                service.Run(args);
                return;
            }
            //This is the normal entry point for the service
            ServiceBase.Run(service);
        }

        public static ServiceProcessInstaller GetServiceProcessInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            return serviceProcessInstaller;
        }

        public static ServiceInstaller GetServiceInstaller(string serviceName)
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.Description = null;
            return serviceInstaller;
        }
    }
}
