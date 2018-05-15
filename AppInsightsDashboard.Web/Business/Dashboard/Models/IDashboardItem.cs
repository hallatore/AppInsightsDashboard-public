using System;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public interface IDashboardItem
    {
        string Name { get; }
        Guid ApplicationId { get; }
        string ApiKey { get; }
        bool Silent { get; }
        DashboardType DashboardType { get; }
    }

    public enum DashboardType
    {
        Site = 0,
        Analytics = 1
    }
}