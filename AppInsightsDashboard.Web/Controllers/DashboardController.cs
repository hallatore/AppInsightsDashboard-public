using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AppInsightsDashboard.Web.Business.Dashboard;
using AppInsightsDashboard.Web.ViewModels.Dashboard;

namespace AppInsightsDashboard.Web.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index(Guid id, int columns = 3)
        {
            var model = new IndexViewModel
            {
                Id = id,
                Columns = columns,
                Items = DashboardHelper.GetItems(id)
            };

            return View(model);
        }

        [OutputCache(Duration = 50, Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Status(Guid id, Guid applicationId)
        {
            var result = await DashboardHelper.GetStatus(id, applicationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}