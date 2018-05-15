using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace AppInsightsDashboard.Web.Business.AppInsightsApi
{
    public static class AppInsightsClient
    {
        // https://www.applicationinsights.microsoft.com/apiexplorer/metrics
        private const string Url = "https://api.applicationinsights.io/v1/apps/{0}/{1}/{2}?timespan={3}&aggregation={4}";
        private const string QueryUrl = "https://api.applicationinsights.io/v1/apps/{0}/query?query={1}";
        private static Dictionary<string, HttpClient> httpClients = new Dictionary<string, HttpClient>();

        private static HttpClient GetHttpClient(string apiKey)
        {
            lock(httpClients)
            {
                if (httpClients.ContainsKey(apiKey))
                    return httpClients[apiKey];

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                httpClients.Add(apiKey, client);
                return client;
            }
        }

        private static async Task<JObject> GetTelemetry(Guid appid, string apikey, string operation, string path, string timespan, string aggregation)
        {
            var httpClient = GetHttpClient(apikey);
            var url = string.Format(Url, appid, operation, path, timespan, aggregation);
            var json = await httpClient.GetStringAsync(url).ConfigureAwait(false);
            return JObject.Parse(json);
        }

        public static async Task<int?> GetTelemetryQuery(Guid appid, string apikey, string query)
        {
            var httpClient = GetHttpClient(apikey);
            var url = string.Format(QueryUrl, appid, HttpUtility.UrlEncode(query));
            var json = await httpClient.GetStringAsync(url).ConfigureAwait(false);
            var result = JObject.Parse(json);
            return result["tables"][0]["rows"][0][0].Value<int?>();
        }

        private static async Task<int?> GetTelemetryAsInt(Guid appid, string apikey, string operation, string path, AppInsightsTimeSpan timespan, string aggregation)
        {
            var result = await GetTelemetry(appid, apikey, "metrics", path, timespan.ToString(), aggregation);
            return result["value"][path][aggregation].Value<int?>();
        }

        private static async Task<long?> GetTelemetryAsLong(Guid appid, string apikey, string operation, string path, AppInsightsTimeSpan timespan, string aggregation)
        {
            var result = await GetTelemetry(appid, apikey, "metrics", path, timespan.ToString(), aggregation);
            return result["value"][path][aggregation].Value<long?>();
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

        public static Task<int?> GetRequestsDurationPercentile(Guid appid, string apikey, double percentile)
        {
            return GetTelemetryQuery(appid, apikey, string.Format("requests | where timestamp >= ago(1h) | summarize percentiles(duration, {0})", percentile));
        }

        public static Task<int?> GetAvailabilityPercentage(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "availabilityResults/availabilityPercentage", timeSpan, "avg");
        }

        public static Task<int?> GetTelemetryCount(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsInt(appid, apikey, "metrics", "billing/telemetryCount", timeSpan, "sum");
        }

        public static Task<long?> GetTelemetrySize(Guid appid, string apikey, AppInsightsTimeSpan timeSpan)
        {
            return GetTelemetryAsLong(appid, apikey, "metrics", "billingMeters/telemetrySize", timeSpan, "sum");
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