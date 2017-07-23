using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

//Keep the namespace to the default although the file is located in App_Start.
namespace Gallery.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Enable attribute based routing for WebApi.
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}