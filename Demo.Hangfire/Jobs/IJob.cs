using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Hangfire.Jobs
{
    public interface IJob
    {
        void Run();
        void Run(string context);
    }
}
