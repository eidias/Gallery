using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gallery.Service
{
    static class Program
    {
        internal const string ConsoleSwitch = "-Console";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            MyService myService = new MyService();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Contains(ConsoleSwitch))
            {
                myService.Run(args);
                return;
            }

            ServiceBase.Run(myService);
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public partial class MyService : ServiceBase
    {
        readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        Thread thread;

        protected override void OnStart(string[] args)
        {
            Run(args);
        }

        protected override void OnStop()
        {
            //Ensure service is properly shut down.
            autoResetEvent.Set();
            thread.Join();
        }

        public void Run(string[] args)
        {
            //Service code goes here.
        }

        protected void TimedRun(Action callback, TimeSpan interval)
        {
            ThreadStart threadStart = () =>
            {
                do
                {
                    callback();
                }
                while (!autoResetEvent.WaitOne(interval));
            };
            thread = new Thread(threadStart);
            thread.Start();
        }
    }
}
