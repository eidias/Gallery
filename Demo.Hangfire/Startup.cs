using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Hangfire
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //The difference is that GetService<T>() returns null if it can't find the service. GetRequiredService<T>() throws an InvalidOperationException if it can't find it.

            services.AddMvc();

            services.AddHangfire();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfire(serviceProvider);

            //Default values that would be applied when calling UseHangfireDashboard() without parameters.
            app.UseHangfireDashboard();



            app.UseMvcWithDefaultRoute();


        }


    }
}
