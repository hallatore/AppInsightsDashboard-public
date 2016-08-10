using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AppInsightsDashboard.Web.Business.AppInsightsApi;
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
        public async Task<ActionResult> Quota(Guid id)
        {
            var apps = new Dictionary<string, int?>();

            foreach (var project in DashboardConfig.Dashboards[id])
            {
                var telemetryCountLastDay = await AppInsightsClient.GetTelemetryCount(project.ApplicationId, project.ApiKey, AppInsightsTimeSpan.P1D);
                apps.Add(project.Name, telemetryCountLastDay);
            }

            return View(apps);
        }

        [OutputCache(Duration = 50, Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Status(Guid id, Guid applicationId)
        {
            var result = await DashboardHelper.GetStatus(id, applicationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}