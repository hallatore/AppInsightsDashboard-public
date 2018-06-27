using System;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public class AnalyticsQuery : ICustomQuery
    {
        public AnalyticsQuery(string name, string query, ApiToken apiToken, Func<dynamic, dynamic> format = null, string postfix = "", Func<dynamic, ErrorLevel> getErrorLevel = null)
        {
            Type = "Value";
            Name = name;
            Query = query;
            ApiToken = apiToken;
            Format = format;
            Postfix = postfix;
            GetErrorLevel = getErrorLevel;
        }
        
        public string Type { get; }
        public string Name { get; }
        public string Query { get; }
        public ApiToken ApiToken { get; }
        public Func<dynamic, dynamic> Format { get; }
        public string Postfix { get; }
        public Func<dynamic, ErrorLevel> GetErrorLevel { get; }

        public async Task<dynamic> GetStatus()
        {
            var result = await AppInsightsClient.TryGetTelemetryQuery(ApiToken.ApplicationId, ApiToken.ApiKey, Query);
            return result;
        }
    }
}