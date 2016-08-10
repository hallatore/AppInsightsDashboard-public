using System;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class DashboardItem
    {
        public DashboardItem(string name, Guid applicationId, string apiKey)
        {
            Name = name;
            ApplicationId = applicationId;
            ApiKey = apiKey;
        }

        public string Name { get; private set; }
        public Guid ApplicationId { get; private set; }
        public string ApiKey { get; private set; }
    }
}