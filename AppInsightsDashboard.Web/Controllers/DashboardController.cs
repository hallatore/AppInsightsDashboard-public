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
        private static DashboardHelper _dashboardHelper;

        public DashboardController()
        {
            if(_dashboardHelper == null) 
                _dashboardHelper = new DashboardHelper(new Config());
        }

        public ActionResult Index(Guid id, int columns = 12)
        {
            var model = new IndexViewModel
            {
                Id = id,
                Columns = columns,
                Items = _dashboardHelper.GetItems(id)
            };

            return View(model);
        }

        [OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Status(Guid id, string name)
        {
            var result = await _dashboardHelper.GetStatus(id, name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}