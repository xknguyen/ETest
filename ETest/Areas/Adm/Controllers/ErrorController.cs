using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETest.Areas.Adm.Controllers
{
    public class ErrorController : AdminController
    {
        public ActionResult ErrorPage(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        public ActionResult AccessDeniedPage(string url)
        {
            ViewBag.Url = url;
            return View();
        }
    }
}