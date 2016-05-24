using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndDev
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,"",new{controller = "Home",action = "Index" });//Title page!

            routes.MapRoute(null, "{controller}/{action}");

            routes.MapRoute(null, "{controller}/{action}/{id}",
                defaults: new {controller = "News", action = "EditList", id = UrlParameter.Optional});


            //routes.MapRoute(name: null, url: "{controller}/Page{page}", defaults: new { action = "Index", category = (string)null }, constraints: new { page = @"\d+" });

            //routes.MapRoute(null, "{controller}/{category}", new { action = "Index", page = 1 });

            //routes.MapRoute(
            //    name: null, url: "{controller}/{action}/Page{page}", defaults: new { controller = "News", action = "Index" }
            //    );

            //routes.MapRoute(null, "{category}/Page{page}", new { action = "Index" },
            //    new { page = @"\d+" });


            //routes.MapRoute(
            //                name: "Default",
            //                url: "{controller}/{action}/{id}",
            //                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //            );
        }
    }
}




















