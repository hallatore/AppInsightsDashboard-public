using System.Threading.Tasks;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards
{
    public interface IDashboard
    {
        bool Silent { get; }
        DashboardType DashboardType { get; }
        Task<IItemStatus> GetStatus();
    }

    public enum DashboardType
    {
        Site = 0,
        Analytics = 1
    }
}