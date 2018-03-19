using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Net
{
    //WebClient descends from System.ComponentModel.Component, which is defined with a [DesignerCategory("Component")] attribute.
    //Decorating our derived class prevents VS from opening it with a designer.
    [DesignerCategory("Code")]
    public class FtpClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var ftpWebRequest = base.GetWebRequest(address) as FtpWebRequest;
            ftpWebRequest.EnableSsl = true;
            return ftpWebRequest;
        }

        public FtpWebResponse GetFtpWebResponse(string remoteAddress, string method)
        {
            var uri = new Uri(remoteAddress);
            var request = GetWebRequest(uri);
            request.Method = method;
            return request.GetResponse() as FtpWebResponse;
        }

        public IEnumerable<string> ListDirectory(string address, bool showDetails = false)
        {
            var method = showDetails ? WebRequestMethods.Ftp.ListDirectoryDetails : WebRequestMethods.Ftp.ListDirectory;
            var response = GetFtpWebResponse(address, method);
            var responseStream = response.GetResponseStream();
            using (var streamReader = new StreamReader(responseStream))
            {
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine();
                }
            }
        }
    }
}
