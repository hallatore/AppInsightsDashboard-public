using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;
using AppInsightsDashboard.Web.Business.Dashboard.Models;

namespace AppInsightsDashboard.Web.Business.Dashboard
{
    public static class DashboardHelper
    {
        public static List<DashboardItem> GetItems(Guid id)
        {
            if (DashboardConfig.Dashboards.ContainsKey(id))
                return DashboardConfig.Dashboards[id];

            return new List<DashboardItem>();
        }

        public static async Task<ItemStatus> GetStatus(Guid id, Guid applicationId)
        {
            var item = DashboardHelper.GetItems(id).First(a => a.ApplicationId == applicationId);
            var requestsCount = AppInsightsClient.GetRequestsCount(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT1H);
            var requestsFailed = AppInsightsClient.GetExceptionsServer(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT1H);
            var requestsDuration = AppInsightsClient.GetRequestsDurationPercentile(item.ApplicationId, item.ApiKey, 90);
            var requestsCount5Min = AppInsightsClient.GetRequestsCount(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT5M);
            var requestsFailed5Min = AppInsightsClient.GetExceptionsServer(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT5M);
            var availabilityPercentage10Min = AppInsightsClient.GetAvailabilityPercentage(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT10M);
            await Task.WhenAll(requestsCount, requestsFailed, requestsDuration, requestsCount5Min, requestsFailed5Min, availabilityPercentage10Min);

            var requests = requestsCount.Result / 60;
            var responseTime = requestsDuration.Result.HasValue ? requestsDuration.Result.Value : 0;
            var errorRate = requestsCount.Result.HasValue && requestsFailed.Result.HasValue && requestsCount.Result.Value > 0 && requestsFailed.Result.Value >= DashboardConfig.Settings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount.Result.Value * requestsFailed.Result.Value, 1) : 0;
            var errorRate5Min = requestsCount5Min.Result.HasValue && requestsFailed5Min.Result.HasValue && requestsCount5Min.Result > 0 && requestsFailed5Min.Result >= DashboardConfig.Settings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount5Min.Result.Value * requestsFailed5Min.Result.Value, 1) : 0;
            var availabilityErrorLevel = availabilityPercentage10Min.Result < 50 ? ErrorLevel.Error : ErrorLevel.Normal;

            var result = new ItemStatus
            {
                RequestsPerMinute = requests.Value,
                AvgResponseTime = responseTime,
                ErrorRate = errorRate,
                ErrorRate5Min = errorRate5Min,
                AvgResponseTimeErrorLevel = responseTime > DashboardConfig.Settings.AvgResponseTimeWarning ? ErrorLevel.Warning : ErrorLevel.Normal,
                ErrorRateLevel = GetErrorRateLevel(errorRate),
                ErrorRateLevel5Min = GetErrorRateLevel(errorRate5Min)
            };

            result.ErrorLevel = (ErrorLevel)new[] { result.AvgResponseTimeErrorLevel, result.ErrorRateLevel, result.ErrorRateLevel5Min, availabilityErrorLevel }.Cast<int>().Max();
            return result;
        }

        private static ErrorLevel GetErrorRateLevel(double errorRate)
        {
            if (errorRate >= DashboardConfig.Settings.ErrorRateError)
                return ErrorLevel.Error;

            if (errorRate >= DashboardConfig.Settings.ErrorRateWarning)
                return ErrorLevel.Warning;

            return ErrorLevel.Normal;
        }
    }
}