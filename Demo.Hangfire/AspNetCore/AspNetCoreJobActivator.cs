using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace Demo.Hangfire.AspNetCore
{
    public class AspNetCoreJobActivator : JobActivator
    {
        readonly IServiceProvider serviceProvider;

        public AspNetCoreJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return serviceProvider.GetService(jobType);
        }
    }
}
