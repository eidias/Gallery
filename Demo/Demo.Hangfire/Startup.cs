using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.AspNetCore;
using Demo.Hangfire.Data;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Hangfire
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Use EF to create the database, so that hangfire can create its own stuff.
            //Use FT insert trigger for Hangfire Job
            //Make FileQueue Scheduler?

            services.AddHangfire(x => x.UseSqlServerStorage(SqlServerHelper.DefaultConnectionString));

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            BackgroundJobServerOptions backgroundJobServerOptions = new BackgroundJobServerOptions
            {
                Activator = new AspNetCoreJobActivator(serviceProvider)
            };
            app.UseHangfireServer(backgroundJobServerOptions);

            app.UseMvcWithDefaultRoute();


        }
    }
}
