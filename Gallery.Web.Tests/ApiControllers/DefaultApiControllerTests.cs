using Gallery.Web.ApiControllers;
using Gallery.Web.Helpers;
using Gallery.Web.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace Gallery.Web.Tests.ApiControllers
{
    public class DefaultApiControllerTests : IClassFixture<WebApiFixture>
    {
        WebApiFixture webApiFixture;

        public DefaultApiControllerTests(WebApiFixture webApiFixture)
        {
            this.webApiFixture = webApiFixture;
        }

        [Fact]
        public void CreateTest()
        {
            DefaultApiController defaultApiController = new DefaultApiController();
            var result = defaultApiController.Status();
            Assert.IsType<OkResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetSingleTest(int id)
        {
            HttpResponseMessage httpResponseMessage = await webApiFixture.SendAsync(HttpMethod.Post, $"api/status/get/{id}");
            List<int> numbers = await httpResponseMessage.Content.ReadAsAsync<List<int>>();
            Assert.Equal(MediaType.ApplicationJson, httpResponseMessage.Content?.Headers.ContentType.MediaType);
            Assert.True(numbers.Contains(id));
        }
    }
}
