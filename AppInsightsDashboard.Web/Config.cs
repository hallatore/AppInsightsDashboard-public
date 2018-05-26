using System;
using System.Collections.Generic;
using AppInsightsDashboard.Web.Business.Dashboard.Models;
using AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards;
using AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries;

namespace AppInsightsDashboard.Web
{
    public class Config
    {
        public Dictionary<Guid, Dictionary<string, IDashboard>> Dashboards { get; }

        public Config()
        {
            var defaultSiteSettings = new SiteSettings
            {
                AvgResponseTimeWarning = 1000,
                ErrorRateWarning = 1.0,
                ErrorRateError = 3.0,
                ErrorCountMinimum = 5
            };

            var demoWebsiteToken = new ApiToken("<Application ID>", "<API key>");

            var mainDashboard = new Dictionary<string, IDashboard>();
            mainDashboard.Add("Website", new DashboardSite(demoWebsiteToken, defaultSiteSettings));
            mainDashboard.Add("Web tests", new DashboardCustom
            {
                Queries = new List<ICustomQuery>
                {
                    new AnalyticsQuery("24h",
                        apiToken: demoWebsiteToken,
                        query: @"
                            availabilityResults
                            | where timestamp > ago(24h)
                            | summarize _successCount=todouble(countif(success == 1)), _totalCount=todouble(count())
                            | project _successCount / _totalCount * 100",
                        postfix: "%",
                        getErrorLevel: (value) =>
                        {
                            if (value < 95)
                                return ErrorLevel.Error;
                            if (value < 99)
                                return ErrorLevel.Warning;
                            if (value == 100)
                                return ErrorLevel.Gray;

                            return ErrorLevel.Normal;
                        }),
                    new AnalyticsQuery("1 hour",
                        apiToken: demoWebsiteToken,
                        query: @"
                            availabilityResults
                            | where timestamp > ago(1h)
                            | summarize _successCount=todouble(countif(success == 1)), _totalCount=todouble(count())
                            | project _successCount / _totalCount * 100",
                        postfix: "%",
                        getErrorLevel: (value) =>
                        {
                            if (value < 95)
                                return ErrorLevel.Error;
                            if (value < 99)
                                return ErrorLevel.Warning;
                            if (value == 100)
                                return ErrorLevel.Gray;

                            return ErrorLevel.Normal;
                        }),
                    new HelloWorldQuery("Hello World")
                }
            });

            Dashboards = new Dictionary<Guid, Dictionary<string, IDashboard>>
            {
                { Guid.Parse("<Random GUID>"), mainDashboard }
            };
        }
    }
}