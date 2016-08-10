using System.Web.Mvc;

namespace AppInsightsDashboard.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}