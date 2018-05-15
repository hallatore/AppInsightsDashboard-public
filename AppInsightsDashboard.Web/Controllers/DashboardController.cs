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

        [OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> SiteStatus(Guid id, Guid applicationId, string name)
        {
            var result = await DashboardHelper.GetSiteStatus(id, applicationId, name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> AnalyticsStatus(Guid id, Guid applicationId, string name)
        {
            var result = await DashboardHelper.GetAnalyticsStatus(id, applicationId, name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}