using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Gallery.Web.Models
{
    public class CachedTrace
    {
        public string CallStack { get; private set; }
        public DateTime DateTime { get; private set; }
        public Stack LogicalOperationStack { get; private set; }
        public int ProcessId { get; private set; }
        public string ThreadId { get; private set; }
        public long Timestamp { get; private set; }
        public string Source { get; private set; }
        public TraceEventType EventType { get; private set; }
        public int Id { get; private set; }
        public string Message { get; private set; }
        public string Format { get; private set; }
        public object[] Args { get; private set; }

        public CachedTrace(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            CallStack = eventCache.Callstack;
            DateTime = eventCache.DateTime;
            LogicalOperationStack = eventCache.LogicalOperationStack;
            ProcessId = eventCache.ProcessId;
            ThreadId = eventCache.ThreadId;
            Timestamp = eventCache.Timestamp;
            Source = source;
            EventType = eventType;
            Id = id;
        }

        public CachedTrace(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
            : this(eventCache, source, eventType, id)
        {
            Message = message;
        }

        public CachedTrace(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
            : this(eventCache, source, eventType, id)
        {
            Format = format;
            Args = args;
        }
    }
}