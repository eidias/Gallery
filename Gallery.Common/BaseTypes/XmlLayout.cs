using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gallery.Common.BaseTypes
{
    public abstract class XmlLayout
    {
        public void Serialize(FileInfo fileInfo)
        {
            using (FileStream fileStream = fileInfo.OpenWrite())
            {
                SerializeToStream(fileStream);
            }
        }

        internal void SerializeToStream(Stream stream)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(GetType());
            //Xml files are by default encoded using UTF-8 even without declaration
            xmlSerializer.Serialize(stream, this);
        }

        public static TLayout Deserialize<TLayout>(FileInfo fileInfo) where TLayout : XmlLayout
        {
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                return DeserializeFromStream<TLayout>(fileStream);
            }
        }

        internal static TLayout DeserializeFromStream<TLayout>(Stream stream) where TLayout : XmlLayout
        {
            //XmlSerializer cannot deserialize to interface types
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TLayout));
            //The class constraint on T allows the use of the 'as' operator 
            return xmlSerializer.Deserialize(stream) as TLayout;
        }
    }
}
