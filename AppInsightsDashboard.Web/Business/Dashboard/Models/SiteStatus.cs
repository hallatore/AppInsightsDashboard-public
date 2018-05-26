namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class SiteStatus : IItemStatus
    {
        public int RequestsPerMinute { get; set; }
        public int AvgResponseTime { get; set; }
        public ErrorLevel AvgResponseTimeErrorLevel { get; set; }
        public double ErrorRate { get; set; }
        public ErrorLevel ErrorRateLevel { get; set; }
        public double ErrorRate10Min { get; set; }
        public ErrorLevel ErrorRateLevel10Min { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
    }
}