using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Xml
{
    public class XmlStringWriter : StringWriter
    {
        Encoding encoding;

        public XmlStringWriter(Encoding encoding)
            : base()
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }

    }
}
