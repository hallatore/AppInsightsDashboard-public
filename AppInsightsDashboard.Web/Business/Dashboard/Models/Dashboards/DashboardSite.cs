using System;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.AppInsightsApi;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards
{
    public class DashboardSite : IDashboard
    {
        public DashboardSite(ApiToken apiToken, SiteSettings siteSettings, bool silent = false)
        {
            ApplicationId = apiToken.ApplicationId;
            ApiKey = apiToken.ApiKey;
            Silent = silent;
            SiteSettings = siteSettings;
            DashboardType = DashboardType.Site;
        }

        public Guid ApplicationId { get; }
        public string ApiKey { get; }
        public bool Silent { get; }
        public SiteSettings SiteSettings { get; }
        public DashboardType DashboardType { get; }

        public async Task<IItemStatus> GetStatus()
        {
            var requestsCount = AppInsightsClient.GetRequestsCount(ApplicationId, ApiKey, AppInsightsTimeSpan.PT1H);
            var requestsFailed = AppInsightsClient.GetExceptionsServer(ApplicationId, ApiKey, AppInsightsTimeSpan.PT1H);
            var requestsDuration = AppInsightsClient.GetRequestsDurationPercentile(ApplicationId, ApiKey, 90);
            var requestsCount10Min = AppInsightsClient.GetRequestsCount(ApplicationId, ApiKey, AppInsightsTimeSpan.PT10M);
            var requestsFailed10Min = AppInsightsClient.GetExceptionsServer(ApplicationId, ApiKey, AppInsightsTimeSpan.PT10M);
            var availabilityPercentage10Min = AppInsightsClient.GetAvailabilityPercentage(ApplicationId, ApiKey, AppInsightsTimeSpan.PT10M);
            await Task.WhenAll(requestsCount, requestsFailed, requestsDuration, requestsCount10Min, requestsFailed10Min, availabilityPercentage10Min);

            var requests = requestsCount.Result / 60;
            var responseTime = requestsDuration.Result ?? 0;
            var errorRate = requestsCount.Result.HasValue && requestsFailed.Result.HasValue && requestsCount.Result.Value > 0 && requestsFailed.Result.Value >= SiteSettings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount.Result.Value * requestsFailed.Result.Value, 1) : 0;
            var errorRate10Min = requestsCount10Min.Result.HasValue && requestsFailed10Min.Result.HasValue && requestsCount10Min.Result > 0 && requestsFailed10Min.Result >= SiteSettings.ErrorCountMinimum ? Math.Round(100.0 / requestsCount10Min.Result.Value * requestsFailed10Min.Result.Value, 1) : 0;
            var availabilityErrorLevel = availabilityPercentage10Min.Result < 50 ? ErrorLevel.Error : ErrorLevel.Normal;

            if (requestsFailed.Result.HasValue && requestsFailed.Result.Value > 0 && (!requestsCount.Result.HasValue || requestsCount.Result.Value == 0))
            {
                errorRate = 100;
            }

            if (requestsFailed10Min.Result.HasValue && requestsFailed10Min.Result.Value > 0 && (!requestsCount10Min.Result.HasValue || requestsCount10Min.Result.Value == 0))
            {
                errorRate10Min = 100;
            }

            var result = new SiteStatus
            {
                RequestsPerMinute = requests ?? 0,
                AvgResponseTime = Convert.ToInt32(responseTime),
                ErrorRate = errorRate,
                ErrorRate10Min = errorRate10Min,
                AvgResponseTimeErrorLevel = responseTime > SiteSettings.AvgResponseTimeWarning ? ErrorLevel.Warning : ErrorLevel.Normal,
                ErrorRateLevel = GetErrorRateLevel(errorRate, SiteSettings),
                ErrorRateLevel10Min = GetErrorRateLevel(errorRate10Min, SiteSettings)
            };

            var errorLevels = new[]
            {
                result.AvgResponseTimeErrorLevel, 
                result.ErrorRateLevel, 
                result.ErrorRateLevel10Min,
                availabilityErrorLevel
            };

            if (errorLevels.Any(r => r == ErrorLevel.Error))
                result.ErrorLevel = ErrorLevel.Error;
            else if (errorLevels.Any(r => r == ErrorLevel.Warning))
                result.ErrorLevel = ErrorLevel.Warning;
            else if (errorLevels.Any(r => r == ErrorLevel.Normal))
                result.ErrorLevel = ErrorLevel.Normal;

            return result;
        }

        private ErrorLevel GetErrorRateLevel(double errorRate, SiteSettings siteSettings)
        {
            if (errorRate >= siteSettings.ErrorRateError)
                return ErrorLevel.Error;

            if (errorRate >= siteSettings.ErrorRateWarning)
                return ErrorLevel.Warning;

            if (errorRate == 0)
                return ErrorLevel.Gray;

            return ErrorLevel.Normal;
        }
    }
}