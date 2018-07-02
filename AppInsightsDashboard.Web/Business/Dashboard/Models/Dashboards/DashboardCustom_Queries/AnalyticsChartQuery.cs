using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public class AnalyticsChartQuery : ICustomQuery
    {
        public AnalyticsChartQuery(string query, TimeSpan interval, TimeSpan duration, ApiToken apiToken, Func<List<double>, List<double>> format, Func<List<double>, ErrorLevel> getErrorLevel)
        {
            Type = "Chart";
            Query = query;
            ApiToken = apiToken;
            _getErrorLevel = getErrorLevel;
            _format = format;
            _interval = interval;
            _duration = duration;
        }
        
        public string Type { get; }
        public string Name { get; }
        public string Query { get; }
        public string Postfix { get; }
        public ApiToken ApiToken { get; }
        private readonly Func<List<double>, List<double>> _format;
        private readonly TimeSpan _interval;
        private readonly TimeSpan _duration;

        public Func<dynamic, dynamic> Format => (input) => _format((List<double>)input);
        private readonly Func<List<double>, ErrorLevel> _getErrorLevel;
        public Func<dynamic, ErrorLevel> GetErrorLevel => (input) => _getErrorLevel((List<double>) input);

        public async Task<dynamic> GetStatus()
        {
            var result = await AppInsightsClient.TryGetTelemetryQueryList(ApiToken.ApplicationId, ApiToken.ApiKey, Query);
            return AddMissingColumns(result);
        }

        private dynamic AddMissingColumns(Dictionary<DateTime, double> columns)
        {
            if (columns == null || !columns.Any())
                return new List<double>();

            var maxDate = columns.Max(c => c.Key);

            while (maxDate < DateTime.UtcNow - _interval)
            {
                maxDate += _interval;
            }

            var minDate = maxDate - _duration;

            while (minDate <= maxDate)
            {
                if (!columns.ContainsKey(minDate))
                    columns.Add(minDate, 0);

                minDate += _interval;
            }

            return columns
                .OrderBy(c => c.Key)
                .Select(c => c.Value)
                .ToList();
        }
    }
}