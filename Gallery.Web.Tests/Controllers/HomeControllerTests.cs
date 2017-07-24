using Gallery.Web.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Gallery.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        void IndexActionTest()
        {
            Mock<HttpRequestBase> mockedHttpRequest = new Mock<HttpRequestBase>();
            mockedHttpRequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection { { "X-Requested-With", "XMLHttpRequest" } });

            //TODO: Implement the proper test.
            //HomeController homeController = MvcTestHelper.CreateController<HomeController>(mockedHttpRequest, new RouteData());
            //var view = homeController.Index();
        }

        [Fact]
        void Foo()
        {
            MockRepository mockRepository = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mockRepository.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("George");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);

            Mock<HttpRequestBase> mockedHttpRequest = new Mock<HttpRequestBase>();
            mockedHttpRequest.SetupGet(x => x.IsAuthenticated).Returns(true);

            Mock<HttpContextBase> mockedHttpContext = new Mock<HttpContextBase>();
            mockedHttpContext.SetupGet(x => x.Request).Returns(mockedHttpRequest.Object);

            Mock<ControllerContext> mockedControllerContext = new Mock<ControllerContext>();
            mockedControllerContext.SetupGet(x => x.HttpContext.User).Returns(mockPrincipal.Object);
            mockedControllerContext.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(false);

            //TODO: Implement the proper test.
            //HomeController homeController = new HomeController() { ControllerContext = mockContext.Object };
            //homeController.Index();
        }
    }
}
