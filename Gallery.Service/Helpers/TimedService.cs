using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gallery.Service.Helpers
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class TimedService : ServiceBase
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
