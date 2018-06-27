using System;
using System.Threading.Tasks;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public interface ICustomQuery
    {
        string Type { get; }
        string Name { get; }
        string Postfix { get; }
        Func<dynamic, ErrorLevel> GetErrorLevel { get; }
        Func<dynamic, dynamic> Format { get; }
        Task<dynamic> GetStatus();
    }
}