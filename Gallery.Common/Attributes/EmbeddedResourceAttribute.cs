using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class EmbeddedResourceAttribute : Attribute
    {
        /// <summary>
        /// Indicates that a method is related to an embedded resource
        /// </summary>
        /// <param name="path">The path to the embedded resource relative to the project path</param>
        public EmbeddedResourceAttribute(string path)
        {
            Name = path.Replace('/', '.');
        }

        public string Name { get; private set; }
    }
}
