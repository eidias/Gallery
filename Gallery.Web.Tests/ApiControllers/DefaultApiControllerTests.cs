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
        public void StatusTest()
        {
            //Implement a way how the controller status can be tested.
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetTest(int id)
        {
            HttpResponseMessage httpResponseMessage = await webApiFixture.SendAsync(HttpMethod.Post, $"api/default/get/{id}");
            var result = await httpResponseMessage.Content.ReadAsAsync<object>();
            Assert.NotNull(result);
        }
    }
}
