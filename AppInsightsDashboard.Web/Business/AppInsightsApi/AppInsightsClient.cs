using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace AppInsightsDashboard.Web.Business.AppInsightsApi
{
    public static class AppInsightsClient
    {
        // https://www.applicationinsights.microsoft.com/apiexplorer/metrics
        private const string Url = "https://api.applicationinsights.io/beta/apps/{0}/{1}/{2}?timespan={3}&aggregation={4}";
        private const string QueryUrl = "https://api.applicationinsights.io/beta/apps/{0}/query?query={1}";

        private static async Task<JObject> GetTelemetry(Guid appid, string apikey, string operation, string path, string timespan, string aggregation)
        {
            var url = string.Format(Url, appid, operation, path, timespan, aggregation);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apikey);
                var json = await client.GetStringAsync(url).ConfigureAwait(false);
                return JObject.Parse(json);
            }
        }

        private static async Task<JObject> GetTelemetryQuery(Guid appid, string apikey, string query)
        {
            var url = string.Format(QueryUrl, appid, HttpUtility.UrlEncode(query));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apikey);
                var json = await client.GetStringAsync(url).ConfigureAwait(false);
                return JObject.Parse(json);
            }
        }

        private static async Task<int?> GetTelemetryAsInt(Guid appid, string apikey, string operation, string path, AppInsightsTimeSpan timespan, string aggregation)
        {
            var result = await GetTelemetry(appid, apikey, "metrics", path, timespan.ToString(), aggregation);
            return result["value"][path][aggregation].Value<int?>();
        }

        public static Task<int?> GetRequestsCount(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "requests/count", timeSpan, "sum");
        }

        public static Task<int?> GetRequestsFailed(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "requests/failed", timeSpan, "sum");
        }

        public static Task<int?> GetExceptionsServer(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "exceptions/count", timeSpan, "sum");
        }

        public static Task<int?> GetRequestsDuration(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "requests/duration", timeSpan, "avg");
        }

        public static async Task<int?> GetRequestsDurationPercentile(Guid appid, string apikey, double percentile)
        {
            var result = await GetTelemetryQuery(appid, apikey, string.Format("requests | where timestamp >= ago(1h) | summarize percentiles(duration, {0})", percentile));
            return result["Tables"][0]["Rows"][0][0].Value<int?>();
        }

        public static Task<int?> GetAvailabilityPercentage(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "availabilityResults/availabilityPercentage", timeSpan, "avg");
        }

        public static Task<int?> GetTelemetryCount(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "billing/telemetryCount", timeSpan, "sum");
        }
    }

    public enum AppInsightsTimeSpan
    {
        PT5M,
        PT10M,
        PT1H,
        P1D
    }
}