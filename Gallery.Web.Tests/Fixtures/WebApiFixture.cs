using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Gallery.Web.Tests.Fixtures
{
    //Fixtures must be in the same assembly as the test that uses them.
    public class WebApiFixture : IDisposable
    {
        HttpServer httpServer;

        public WebApiFixture()
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            //Ensures configuration is consitent with API project and is loaded for IAssembliesResolver to find all the controllers which implement the IHttpController interface.
            WebApiConfig.Register(httpConfiguration);
            httpServer = new HttpServer(httpConfiguration);
        }

        public void Dispose()
        {
            httpServer?.Dispose();
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, HttpContent content = null, string mediaType = "application/json")
        {
            //As this server runs truly in memory the base address is simply ignored.
            UriBuilder uriBuilder = new UriBuilder("http://can.be.anything");
            uriBuilder.Path = path;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uriBuilder.Uri);
            httpRequestMessage.Content = content;

            MediaTypeWithQualityHeaderValue item = new MediaTypeWithQualityHeaderValue(mediaType);
            httpRequestMessage.Headers.Accept.Add(item);

            HttpClient httpClient = new HttpClient(httpServer);
            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
