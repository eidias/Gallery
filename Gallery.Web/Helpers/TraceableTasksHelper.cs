using Gallery.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Gallery.Web.Helpers
{
    public class TraceableTaskHelper
    {
        private TraceSource traceSource;
        private HttpSessionStateBase httpSessionStateBase;

        public IEnumerable<Thread> GetThreads()
        {
            return httpSessionStateBase["Threads"] as IEnumerable<Thread>;
        }
        public IEnumerable<Thread> GetAliveThreads()
        {
            return GetThreads().Where(x => x.IsAlive);
        }
        public IEnumerable<CachedTrace> GetCachedTraces(Thread thread)
        {
            string key = String.Format("P{0}T{1}", Process.GetCurrentProcess().Id, thread.ManagedThreadId);
            return HttpRuntime.Cache.Get(key) as IEnumerable<CachedTrace>;
        }
        public TraceableTaskHelper(HttpSessionStateBase httpSessionStateBase)
        {
            traceSource = new TraceSource("MySource", SourceLevels.All);
            this.httpSessionStateBase = httpSessionStateBase;
        }
        public void Start(Action action)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                List<Thread> threads = httpSessionStateBase["Threads"] as List<Thread> ?? new List<Thread>();
                threads.Add(Thread.CurrentThread);
                httpSessionStateBase["Threads"] = threads;
                traceSource.TraceEvent(TraceEventType.Start, 0, "The operation has started.");
                action();
            }, TaskCreationOptions.LongRunning);
            task.ContinueWith(previousTask =>
            {
                try
                {
                    previousTask.Wait();
                }
                catch (AggregateException aggregateException)
                {
                    aggregateException.Handle(exception =>
                    {
                        //Create GetInnermostException() as extension to System.Exception
                        traceSource.TraceEvent(TraceEventType.Error, 0, exception.Message);
                        return true;
                    });
                }
                traceSource.TraceEvent(TraceEventType.Stop, 0, "The operation has ended.");
            }, TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}