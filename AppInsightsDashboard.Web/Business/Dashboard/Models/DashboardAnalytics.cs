using System;
using System.Collections.Generic;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class DashboardAnalytics : IDashboardItem
    {
        public DashboardAnalytics(string name, Guid applicationId, string apiKey, bool silent = false)
        {
            Name = name;
            ApplicationId = applicationId;
            ApiKey = apiKey;
            Silent = silent;
            DashboardType = DashboardType.Analytics;
        }

        public string Name { get; private set; }
        public Guid ApplicationId { get; private set; }
        public string ApiKey { get; private set; }
        public bool Silent { get; private set; }
        public DashboardType DashboardType { get; private set; }
        public List<AnalyticsQuery> Queries { get; set; }
    }

    public class AnalyticsQuery
    {
        public AnalyticsQuery(string name, string query, string postfix = "", Func<int?, ErrorLevel> getErrorLevel = null)
        {
            Name = name;
            Query = query;
            Postfix = postfix;
            GetErrorLevel = getErrorLevel;
        }

        public string Name { get; private set; }
        public string Query { get; private set; }
        public string Postfix { get; private set; }
        public Func<int?, ErrorLevel> GetErrorLevel { get; private set; }
    }
}