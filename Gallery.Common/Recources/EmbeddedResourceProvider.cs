using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gallery.Common.Recources
{
    public class EmbeddedResourceProvider
    {
        readonly Assembly assembly;

        public EmbeddedResourceProvider(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public string[] ManifestResourceNames => assembly.GetManifestResourceNames();

        public Stream this[string manifestResourceName]
        {
            get
            {
                if (ManifestResourceNames.Contains(manifestResourceName))
                {
                    return assembly.GetManifestResourceStream(manifestResourceName);
                }
                throw new KeyNotFoundException("The specified resource could not be found. Make sure the item is compiled as embedded resource.");
            }
        }

        public string GetAnsiText(string manifestResourceName)
        {
            //Standard ANSI Latin 1; Western European (Windows)
            Encoding encoding = Encoding.GetEncoding(1252);
            return GetText(manifestResourceName, encoding);
        }

        public string GetText(string manifestResourceName, Encoding encoding)
        {
            //StreamReader disposes the underlying stream when getting disposed so we do not have to take care of this.
            using (StreamReader streamReader = new StreamReader(this[manifestResourceName], encoding))
            {
                return streamReader.ReadToEnd();
            }
        }

        public T GetSerializedType<T>(string manifestResourceName) where T : class
        {
            Stream stream = this[manifestResourceName] as Stream;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return xmlSerializer.Deserialize(stream) as T;
        }
    }
}
