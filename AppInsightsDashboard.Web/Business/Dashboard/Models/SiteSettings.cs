namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class SiteSettings
    {
        public int AvgResponseTimeWarning { get; set; }
        public double ErrorRateWarning { get; set; }
        public double ErrorRateError { get; set; }
        public int ErrorCountMinimum { get; set; }
    }
}