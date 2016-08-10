using System;
using System.Collections.Generic;
using AppInsightsDashboard.Web.Business.Dashboard.Models;

namespace AppInsightsDashboard.Web
{
    public static class DashboardConfig
    {
        public static readonly DashboardSettings Settings = new DashboardSettings
        {
            AvgResponseTimeWarning = 500,
            ErrorRateWarning = 1.0,
            ErrorRateError = 3.0,
            ErrorCountMinimum = 5
        };

        public static readonly Dictionary<Guid, List<DashboardItem>> Dashboards = new Dictionary<Guid, List<DashboardItem>>
        {
            {
                Guid.Parse("<GUID>"),
                new List<DashboardItem>
                {
                    new DashboardItem("nettside.url.no", new Guid("<application-id>"), "<api-key>")
                }
            }
        };
    }
}