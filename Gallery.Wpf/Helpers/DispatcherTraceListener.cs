using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gallery.Wpf.Helpers
{
    class DispatcherTraceListener : TraceListener
    {
        readonly Action<string> action;
        readonly Func<bool> predicate;

        internal DispatcherTraceListener(Action<string> action)
            : this(action, () => true)
        {
        }

        internal DispatcherTraceListener(Action<string> action, Func<bool> predicate)
        {
            this.action = action;
            this.predicate = predicate;
        }

        public override void Write(string message)
        {
            Dispatch(message);
        }

        public override void WriteLine(string message)
        {
            Dispatch(message);
        }

        void Dispatch(string message)
        {
            if (predicate())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    action(message);
                });
            }
        }
    }
}
