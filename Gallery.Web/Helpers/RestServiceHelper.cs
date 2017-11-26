using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Web.Helpers
{
    public class RestServiceHelper
    {
        public static HttpResponseMessage SendFile(object fileItem, string authHeader = null)
        {
            using (HttpClient httpClient = CreateClient(authHeader))
            {
                MediaTypeWithQualityHeaderValue mediaTypeWithQualityHeaderValue = new MediaTypeWithQualityHeaderValue("application/bson");
                httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);

                // POST using the BSON formatter.
                MediaTypeFormatter mediaTypeFormatter = new BsonMediaTypeFormatter();
                Uri uri = new Uri("http://localhost/api/queue/add");
                return httpClient.PostAsync(uri, fileItem, mediaTypeFormatter).Result;
            }
        }

        public static HttpClient CreateClient(string authHeader = null)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            if (authHeader != null)
            {
                byte[] authenticationBytes = Encoding.ASCII.GetBytes(authHeader);
                string parameter = Convert.ToBase64String(authenticationBytes);
                AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Basic", parameter);
                httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            else
            {
                httpClientHandler.UseDefaultCredentials = true;
            }
            return httpClient;
        }
    }
}
