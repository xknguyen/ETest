using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.XPath;

namespace ETest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Dành cho khóa học
            routes.MapRoute(
            "Course",
            "khoa-hoc.html",
            new { controller = "Home", action = "Course", id = UrlParameter.Optional }
            );

            // Dành cho tests
            routes.MapRoute(
            "Tests",
            "bai-kiem-tra-p{courseId}.html",
            new { controller = "Home", action = "Tests", courseId = UrlParameter.Optional }
            );

            // Dành cho test
            routes.MapRoute(
            "Test",
            "lam-kiem-tra/{id}",
            new { controller = "Home", action = "Test", id = UrlParameter.Optional }
            );

            // Dành cho ChangePassword
            routes.MapRoute(
            "ChangePassword",
            "doi-mat-khau.html",
            new { controller = "Manage", action = "ChangePassword"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
               namespaces: new[] {"ETest.Controllers"}
                );
           // Clear xong rebuild laij thuwr :v 
        }
    }
}
