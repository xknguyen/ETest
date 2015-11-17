using System.Web.Mvc;
using ETest.DAL;

namespace ETest.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected ETestDbContext DbContext = new ETestDbContext();
        public virtual ActionResult RedirectErrorPage(string url)
        {
            return RedirectToAction("ErrorPage", "Error", new { url });
        }

        public virtual ActionResult RedirectAccessDeniedPage(string url)
        {
            return RedirectToAction("AccessDeniedPage", "Error", new { url });
        }

    }
}