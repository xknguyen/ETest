using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETest.Areas.Adm.Controllers
{
    public class DashboardController : AdminController
    {
        // GET: Adm/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}