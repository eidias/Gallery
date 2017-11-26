using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.Data;
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
            string fileTableRootPath = SqlServerHelper.GetFileTableRootPath(connectionString);
            DirectoryInfo directoryInfo = new DirectoryInfo($"{fileTableRootPath}\\{name}");
            if (directoryInfo.Exists)
            {
                StaticFileOptions staticFileOptions = new StaticFileOptions
                {
                    RequestPath = new PathString($"/{name}"),
                    FileProvider = new PhysicalFileProvider(directoryInfo.FullName)
                };
                app.UseStaticFiles(staticFileOptions);
            }
        }
    }
}
