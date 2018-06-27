using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public class AnalyticsChartQuery : ICustomQuery
    {
        public AnalyticsChartQuery(string name, string query, ApiToken apiToken, Func<List<double>, List<double>> format = null, Func<List<double>, ErrorLevel> getErrorLevel = null)
        {
            Type = "Chart";
            Name = name;
            Query = query;
            ApiToken = apiToken;
            _getErrorLevel = getErrorLevel;
            _format = format;
        }
        
        public string Type { get; }
        public string Name { get; }
        public string Query { get; }
        public string Postfix { get; }
        public ApiToken ApiToken { get; }
        private readonly Func<List<double>, List<double>> _format;
        public Func<dynamic, dynamic> Format => (input) => _format((List<double>)input);
        private readonly Func<List<double>, ErrorLevel> _getErrorLevel;
        public Func<dynamic, ErrorLevel> GetErrorLevel => (input) => _getErrorLevel((List<double>) input);

        public async Task<dynamic> GetStatus()
        {
            var result = await AppInsightsClient.TryGetTelemetryQueryList(ApiToken.ApplicationId, ApiToken.ApiKey, Query);
            return result;
        }
    }
}