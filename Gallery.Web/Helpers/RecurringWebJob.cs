using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Gallery.Web.Helpers
{
    class RecurringWebJob : IRegisteredObject
    {
        readonly System.Timers.Timer timer = new System.Timers.Timer();
        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Starts the timer to run the supplied job at the given interval.
        /// </summary>
        /// <param name="interval">Interval in which the event is triggered.</param>
        /// <param name="action">Job to be executed when triggered. Signal time and a cancellation token is available in the parameter.</param>
        public void Start(TimeSpan interval, Action<DateTime, CancellationToken> action)
        {
            timer.Interval = interval.Milliseconds;
            //Elapsed events are raised on a ThreadPool thread and might be raised again if processing lasts longer than "interval".
            timer.Elapsed += (sender, e) =>
            {
                action(e.SignalTime, cancellationTokenSource.Token);

                //Signals the hosting environment that it does not have to further wait on the task to complete.
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    HostingEnvironment.UnregisterObject(this);
                }
            };
            timer.Start();
        }

        /// <summary>
        /// Stops the timer and optionally signals to a running task that it has to terminate immediately.
        /// Automatically called by the hosting environment before the app domain is unloaded for example when an app pool is recycled.
        /// </summary>
        /// <param name="immediate">Signal to a running task that cancellation was requested.</param>
        public void Stop(bool immediate)
        {
            timer.Stop();
            //The hosting environment calls Stop() twice, the second time its called with "immediate" set to true;
            if (immediate)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }

    class RecurringWebJobImplementation
    {
        public void Run()
        {
            RecurringWebJob recurringWebJob = new RecurringWebJob();
            //The hosting environment will call the Stop() method on every registered object before the app domain is unloaded.
            HostingEnvironment.RegisterObject(recurringWebJob);
            recurringWebJob.Start(TimeSpan.FromMinutes(5), (signalTime, cancellationToken) =>
            {
                //Your job implementation. Make sure you properly handle the cancellation token.
            });

        }
    }
}