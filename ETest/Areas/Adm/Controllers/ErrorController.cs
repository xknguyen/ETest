using System.Web.Mvc;
using ETest.Controllers;

namespace Controllers
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