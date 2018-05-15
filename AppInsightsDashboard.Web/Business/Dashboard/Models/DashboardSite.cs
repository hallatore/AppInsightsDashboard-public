using System;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class DashboardSite : IDashboardItem
    {
        public DashboardSite(string name, Guid applicationId, string apiKey, bool silent = false)
        {
            Name = name;
            ApplicationId = applicationId;
            ApiKey = apiKey;
            Silent = silent;
            DashboardType = DashboardType.Site;
        }

        public string Name { get; private set; }
        public Guid ApplicationId { get; private set; }
        public string ApiKey { get; private set; }
        public bool Silent { get; private set; }
        public DashboardType DashboardType { get; private set; }
    }
}