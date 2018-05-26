using System;
using System.Threading.Tasks;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    public interface ICustomQuery
    {
        string Name { get; }
        string Format { get; }
        string Postfix { get; }
        Func<double?, ErrorLevel> GetErrorLevel { get; }

        Task<double?> GetStatus();
    }
}