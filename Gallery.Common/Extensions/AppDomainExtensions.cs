using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class AppDomainExtensions
    {
        public static Type GetNamedType(this AppDomain appDomain, string name)
        {
            //We assume that there is only one assembly containing a particular type in the same namespace.
            Assembly assembly = appDomain.GetAssemblies().Where(x => x.GetType(name) != null).SingleOrDefault();
            //If no assembly is found containing the specified type return null.
            return assembly != null ? assembly.GetType(name) : null;
        }
    }
}
