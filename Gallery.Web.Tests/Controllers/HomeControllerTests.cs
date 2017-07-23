using Gallery.Web.Controllers;
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
            HomeController homeController = MvcTestHelper.CreateController<HomeController>(mockedHttpRequest, new RouteData());
            var view = homeController.Index();
        }

        [Fact]
        void Foo()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("George");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(true); // or false

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(false);

            // create controller
            var controller = new HomeController() { ControllerContext = mockContext.Object };
            controller.Index();
        }
    }
}
