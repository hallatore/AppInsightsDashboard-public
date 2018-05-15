using System.Collections.Generic;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class AnalyticsStatus
    {
        public AnalyticsStatus()
        {
            Values = new List<AnalyticsItem>();
        }

        public ErrorLevel ErrorLevel { get; set; }
        public List<AnalyticsItem> Values { get; set; }
    }

    public class AnalyticsItem
    {
        public string Name { get; set; }
        public int? Value { get; set; }
        public string Postfix { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
    }
}