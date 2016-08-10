namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class ItemStatus
    {
        public int RequestsPerMinute { get; set; }
        public int AvgResponseTime { get; set; }
        public ErrorLevel AvgResponseTimeErrorLevel { get; set; }
        public double ErrorRate { get; set; }
        public ErrorLevel ErrorRateLevel { get; set; }
        public double ErrorRate5Min { get; set; }
        public ErrorLevel ErrorRateLevel5Min { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
    }
}