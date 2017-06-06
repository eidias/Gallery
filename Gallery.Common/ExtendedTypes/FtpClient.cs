using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.ExtendedTypes
{
    //WebClient descends from System.ComponentModel.Component, which is defined with a [DesignerCategory("Component")] attribute.
    //Decorating our derived class prevents VS from opening it with a designer.
    [DesignerCategory("Code")]
    public class FtpClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            FtpWebRequest ftpWebRequest = base.GetWebRequest(address) as FtpWebRequest;
            ftpWebRequest.EnableSsl = true;
            return ftpWebRequest;
        }
    }
}
