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

            //A new empty database with the name supplied in the connection string must be created prior to the first start.
            //By default Hangfire is using its own database schema so it does not interfere with others.
            services.AddHangfire(x => x.UseSqlServerStorage(SqlServerHelper.DefaultConnectionString));


        }
    }
}
