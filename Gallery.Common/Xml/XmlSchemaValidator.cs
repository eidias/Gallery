using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Gallery.Common.Xml
{
    public class XmlSchemaValidator
    {
        const string schemaExtension = ".xsd";

        XmlSchemaSet schemas;
        List<ValidationEventArgs> validationEventArgs;

        public XmlSchemaValidator()
        {
            Assembly assembly = GetType().Assembly;
            IEnumerable<string> schemaResourceNames = assembly.GetManifestResourceNames().Where(x => x.EndsWith(schemaExtension));
            schemas = new XmlSchemaSet();
            foreach (string schemaResourceName in schemaResourceNames)
            {
                //We only load the schema from resource, and assume that now validation action has to be taken.
                XmlSchema xmlSchema = XmlSchema.Read(assembly.GetManifestResourceStream(schemaResourceName), (s, e) => { });
                schemas.Add(xmlSchema);
            }
        }

        public List<ValidationEventArgs> ValidationEventArgs
        {
            get
            {
                return validationEventArgs;
            }
        }

        public void Validate(TextReader textReader)
        {
            validationEventArgs = new List<ValidationEventArgs>();
            XDocument document = XDocument.Load(textReader);
            document.Validate(schemas, (s, e) =>
            {
                validationEventArgs.Add(e);
            });
        }
    }
}
