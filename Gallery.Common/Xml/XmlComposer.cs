using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Gallery.Common.Xml
{
    public class XmlComposer
    {
        public static void SerializeWithEnumerator<TItem>(XElement rootElement, IEnumerable<TItem> items, Stream stream, Action<TItem> onSerializing = null)
        {
            var xDocument = new XDocument();
            xDocument.Add(rootElement);

            var enumerator = items.GetEnumerator();
            //Sets the enumerator to its initial position, which is before the first element in the collection.
            enumerator.Reset();

            var xmlSerializer = new XmlSerializer(typeof(TItem));
            var xmlSerializerNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var xmlWriter = rootElement.CreateWriter())
            {
                while (enumerator.MoveNext())
                {
                    onSerializing?.Invoke(enumerator.Current);

                    //Prevents an exception as WriteStartDocument cannot be called on writers created with ConformanceLevel.Fragment
                    xmlWriter.WriteWhitespace(string.Empty);
                    xmlSerializer.Serialize(xmlWriter, enumerator.Current, xmlSerializerNamespaces);
                }
            }
            xDocument.Save(stream);
        }
    }
}
