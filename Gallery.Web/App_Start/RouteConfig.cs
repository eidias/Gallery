using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


//Keep the namespace to the default although the file is located in App_Start.
namespace Gallery.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //Resource files need to be handled by the appropriate HttpHandler and not MVC.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Enable attribute based routing for MVC.
            routes.MapMvcAttributeRoutes();

            //The combination of route templates and route attributes should be discussed.
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}