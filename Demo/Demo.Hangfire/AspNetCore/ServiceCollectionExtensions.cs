using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.Data;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Hangfire.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHangfire(this IServiceCollection services)
        {
            //Use EF to create the database, so that hangfire can create its own stuff.
            //Use FT insert trigger for Hangfire Job
            //Make FileQueue Scheduler?

            services.AddHangfire(x => x.UseSqlServerStorage(SqlServerHelper.DefaultConnectionString));


        }
    }
}
