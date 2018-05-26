using System;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public class AnalyticsQuery : ICustomQuery
    {
        public AnalyticsQuery(string name, string query, ApiToken apiToken, string format = "{0:0}", string postfix = "", Func<double?, ErrorLevel> getErrorLevel = null)
        {
            Name = name;
            Query = query;
            ApiToken = apiToken;
            Format = format;
            Postfix = postfix;
            GetErrorLevel = getErrorLevel;
        }

        public string Name { get; }
        public string Query { get; }
        public ApiToken ApiToken { get; }
        public string Format { get; }
        public string Postfix { get; }
        public Func<double?, ErrorLevel> GetErrorLevel { get; }

        public Task<double?> GetStatus()
        {
            return AppInsightsClient.TryGetTelemetryQuery(ApiToken.ApplicationId, ApiToken.ApiKey, Query);
        }
    }
}