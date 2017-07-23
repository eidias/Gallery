using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gallery.Web.Tests.Helpers
{
    class MvcTestHelper
    {
        internal static TController CreateController<TController>(Mock<HttpRequestBase> mockedHttpRequest, RouteData routeData) where TController : Controller, new()
        {
            Mock<HttpContextBase> mockedHttpContext = new Mock<HttpContextBase>();
            mockedHttpContext.SetupGet(x => x.Request).Returns(mockedHttpRequest.Object);

            //Need to add System.Net.Mvc to the test class to get access to the ControllerContext
            TController controller = new TController();
            controller.ControllerContext = new ControllerContext(mockedHttpContext.Object, routeData, controller);
            return controller;
        }

    }
}
