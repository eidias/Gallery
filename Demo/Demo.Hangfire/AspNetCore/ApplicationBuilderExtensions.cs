using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.Data;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Demo.Hangfire.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseFileTable(this IApplicationBuilder app, string connectionString)
        {
            UseFileTable(app, connectionString, "Files");
        }

        public static void UseFileTable(this IApplicationBuilder app, string connectionString, string name)
        {
            var fileTableRootPath = SqlServerHelper.GetFileTableRootPath(connectionString);

            var directoryInfo = new DirectoryInfo($"{fileTableRootPath}\\{name}");
            if (directoryInfo.Exists)
            {
                var staticFileOptions = new StaticFileOptions
                {
                    RequestPath = new PathString($"/{name}"),
                    FileProvider = new PhysicalFileProvider(directoryInfo.FullName)
                };
                app.UseStaticFiles(staticFileOptions);
            }
        }

        public static void UseHangfire(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var backgroundJobServerOptions = new BackgroundJobServerOptions
            {
                Activator = new AspNetCoreJobActivator(serviceProvider)
            };
            app.UseHangfireServer(backgroundJobServerOptions);
        }

        public static void UseHangfireDashboard(this IApplicationBuilder app, string pathMatch = "/hangfire", string appPath = "/", int statsPollingInterval = 2000)
        {
            //Restrict access to dashboard to local requests only.
            IDashboardAuthorizationFilter dashboardAuthorizationFilter = new LocalRequestsOnlyAuthorizationFilter();

            var dashboardOptions = new DashboardOptions
            {
                AppPath = appPath,
                Authorization = new[] { dashboardAuthorizationFilter },
                StatsPollingInterval = statsPollingInterval
            };
            app.UseHangfireDashboard(pathMatch, dashboardOptions);
        }
    }
}
