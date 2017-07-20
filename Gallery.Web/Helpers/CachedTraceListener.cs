using Gallery.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Gallery.Web.Helpers
{
    public class CachedTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            throw new NotImplementedException("Use TraceEvent method insted.");
        }
        public override void WriteLine(string message)
        {
            throw new NotImplementedException("Use TraceEvent method insted.");
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            Add(new CachedTrace(eventCache, source, eventType, id));
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            Add(new CachedTrace(eventCache, source, eventType, id, message));
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            Add(new CachedTrace(eventCache, source, eventType, id, format, args));
        }
        private void Add(CachedTrace cachedTrace)
        {
            string key = String.Format("P{0}T{1}", cachedTrace.ProcessId, cachedTrace.ThreadId);
            List<CachedTrace> cachedTraces = HttpRuntime.Cache.Remove(key) as List<CachedTrace> ?? new List<CachedTrace>();
            cachedTraces.Add(cachedTrace);
            HttpRuntime.Cache.Insert(key, cachedTraces, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2));
        }
    }
}