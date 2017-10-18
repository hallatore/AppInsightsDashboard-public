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
            var requestsCount10Min = AppInsightsClient.GetRequestsCount(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT10M);
            var requestsFailed10Min = AppInsightsClient.GetExceptionsServer(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT10M);
            var availabilityPercentage10Min = AppInsightsClient.GetAvailabilityPercentage(item.ApplicationId, item.ApiKey, AppInsightsTimeSpan.PT10M);
            await Task.WhenAll(requestsCount, requestsFailed, requestsDuration, requestsCount10Min, requestsFailed10Min, availabilityPercentage10Min);

            var requests = requestsCount.Result / 60;
            var responseTime = requestsDuration.Result.HasValue ? requestsDuration.Result.Value : 0;
            var errorRate = requestsCount.Result.HasValue && requestsFailed.Result.HasValue && requestsCount.Result.Value > 0 && requestsFailed.Result.Value >= DashboardConfig.Settings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount.Result.Value * requestsFailed.Result.Value, 1) : 0;
            var errorRate10Min = requestsCount10Min.Result.HasValue && requestsFailed10Min.Result.HasValue && requestsCount10Min.Result > 0 && requestsFailed10Min.Result >= DashboardConfig.Settings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount10Min.Result.Value * requestsFailed10Min.Result.Value, 1) : 0;
            var availabilityErrorLevel = availabilityPercentage10Min.Result < 50 ? ErrorLevel.Error : ErrorLevel.Normal;

            if (requestsFailed.Result.HasValue && requestsFailed.Result.Value > 0 && (!requestsCount.Result.HasValue || requestsCount.Result.Value == 0))
            {
                errorRate = 100;
            }

            if (requestsFailed10Min.Result.HasValue && requestsFailed10Min.Result.Value > 0 && (!requestsCount10Min.Result.HasValue || requestsCount10Min.Result.Value == 0))
            {
                errorRate10Min = 100;
            }

            var result = new ItemStatus
            {
                RequestsPerMinute = requests.Value,
                AvgResponseTime = responseTime,
                ErrorRate = errorRate,
                ErrorRate10Min = errorRate10Min,
                AvgResponseTimeErrorLevel = responseTime > DashboardConfig.Settings.AvgResponseTimeWarning ? ErrorLevel.Warning : ErrorLevel.Normal,
                ErrorRateLevel = GetErrorRateLevel(errorRate),
                ErrorRateLevel10Min = GetErrorRateLevel(errorRate10Min)
            };

            result.ErrorLevel = (ErrorLevel)new[] { result.AvgResponseTimeErrorLevel, result.ErrorRateLevel, result.ErrorRateLevel10Min, availabilityErrorLevel }.Cast<int>().Max();
            return result;
        }

        private static ErrorLevel GetErrorRateLevel(double errorRate)
        {
            if (errorRate >= DashboardConfig.Settings.ErrorRateError)
                return ErrorLevel.Error;

            if (errorRate >= DashboardConfig.Settings.ErrorRateWarning)
                return ErrorLevel.Warning;

            if (errorRate == 0)
                return ErrorLevel.Gray;

            return ErrorLevel.Normal;
        }
    }
}