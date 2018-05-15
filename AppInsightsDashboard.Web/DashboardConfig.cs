using System;
using System.Collections.Generic;
using AppInsightsDashboard.Web.Business.Dashboard.Models;

namespace AppInsightsDashboard.Web
{
    public static class DashboardConfig
    {
        public static readonly DashboardSettings Settings = new DashboardSettings
        {
            AvgResponseTimeWarning = 1000,
            ErrorRateWarning = 1.0,
            ErrorRateError = 3.0,
            ErrorCountMinimum = 5
        };

        public static readonly Dictionary<Guid, List<IDashboardItem>> Dashboards = new Dictionary<Guid, List<IDashboardItem>>
        {
            {
                Guid.Parse("<GUID>"),
                new List<IDashboardItem>
                {
                    new DashboardSite("nettside.url.no", new Guid("<application-id>"), "<api-key>"),
                    new DashboardAnalytics("web tests", new Guid("<application-id>"), "<api-key>")
                    {
                        Queries = new List<AnalyticsQuery>
                        {
                            new AnalyticsQuery("24h",
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
                    }
                }
            }
        };
    }
}