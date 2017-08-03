using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Helpers
{
    public static class EnvironmentHelper
    {
        public static void ExtendPath(string path)
        {
            ExtendPath(new string[] { path });
        }

        public static void ExtendPath(IEnumerable<string> paths)
        {
            IEnumerable<string> existingPathSegments = Environment.GetEnvironmentVariable("Path").Split(';').OfType<string>();
            string[] combinedPaths = existingPathSegments.Union(paths).ToArray();

            Environment.SetEnvironmentVariable("Path", String.Join(";", combinedPaths));
        }

        public static string GetVersion()
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            //Easily identify versions released to public from internal versions.
            return assemblyVersion.Major > 0 ? $"Build {assemblyVersion.Build}" : $"Version {assemblyVersion.ToString()}";
        }
    }
}