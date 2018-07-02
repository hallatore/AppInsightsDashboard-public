using System;
using System.Collections.Generic;
using System.Linq;
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
                AvgResponseTimeWarning = 2000,
                ErrorRateWarning = 3.0,
                ErrorRateError = 5.0,
                ErrorCountMinimum = 5
            };
            
            var demoSite = new ApiToken("<guid>", "<key>");

            var mainDashboard = new Dictionary<string, IDashboard>();
            mainDashboard.Add("site", new DashboardSite(demoSite, defaultSiteSettings, true));

            mainDashboard.Add("site - exceptions", new DashboardCustom
            {
                Queries = new List<ICustomQuery>
                {
                    new AnalyticsQuery("redis",
                        apiToken: demoSite,
                        query: @"
                            exceptions
                            | where timestamp > ago(1h)
                            | where type contains 'Redis'
                            | summarize Cabin_SelectionRPM = todouble(count())",
                        postfix: "errors",
                        getErrorLevel: (value) =>
                        {
                            if (value == 0)
                                return ErrorLevel.Gray;

                            return ErrorLevel.Normal;
                        }),
                    new AnalyticsQuery("Task Canceled",
                        apiToken: demoSite,
                        query: @"
                            exceptions
                            | where timestamp > ago(1h)
                            | where type contains 'TaskCanceledException'
                            | summarize Cabin_SelectionRPM = todouble(count())",
                        postfix: "errors",
                        getErrorLevel: (value) =>
                        { 
                            if (value >= 50)
                                return ErrorLevel.Error;
                            if (value >= 20)
                                return ErrorLevel.Warning;
                            if (value == 0)
                                return ErrorLevel.Gray;

                            return ErrorLevel.Normal;
                        })
                }
            });

            mainDashboard.Add("Web tests", new DashboardCustom
            {
                Queries = new List<ICustomQuery>
                {
                    new AnalyticsQuery("24h",
                        apiToken: demoSite,
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
                        apiToken: demoSite,
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
                        })
                }
            });

            Dashboards = new Dictionary<Guid, Dictionary<string, IDashboard>>
            {
                { Guid.Parse("7fd512f1-d1a0-4353-92da-50f02207d70e"), mainDashboard }
            };
        }
    }
}