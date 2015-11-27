using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETest.Controllers
{
    public class ErrorController : BaseController
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