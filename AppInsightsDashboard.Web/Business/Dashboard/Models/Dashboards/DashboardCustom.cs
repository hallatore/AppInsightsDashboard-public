using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards
{
    public class DashboardCustom : IDashboard
    {
        public DashboardCustom(bool silent = false)
        {
            Silent = silent;
            DashboardType = DashboardType.Analytics;
        }

        public bool Silent { get; }
        public DashboardType DashboardType { get; }
        public List<ICustomQuery> Queries { get; set; }

        public async Task<IItemStatus> GetStatus()
        {
            var tasks = new List<Task<dynamic>>();

            foreach (var query in Queries)
            {
                tasks.Add(query.GetStatus());
            }

            await Task.WhenAll(tasks);
            var result = new AnalyticsStatus();

            for (var i = 0; i < Queries.Count; i++)
            {
                result.Values.Add(new AnalyticsItem
                {
                    Type = Queries[i].Type,
                    Name = Queries[i].Name,
                    Value = Queries[i].Format != null ? Queries[i].Format(tasks[i].Result) : tasks[i].Result,
                    Postfix = Queries[i].Postfix,
                    ErrorLevel = GetErrorRateLevel(tasks[i].Result, Queries[i])
                });
            }

            if (result.Values.Any(r => r.ErrorLevel == ErrorLevel.Error))
                result.ErrorLevel = ErrorLevel.Error;
            else if (result.Values.Any(r => r.ErrorLevel == ErrorLevel.Warning))
                result.ErrorLevel = ErrorLevel.Warning;
            else if (result.Values.Any(r => r.ErrorLevel == ErrorLevel.Normal))
                result.ErrorLevel = ErrorLevel.Normal;

            return result;
        }

        private ErrorLevel GetErrorRateLevel(dynamic value, ICustomQuery query)
        {
            if (query.GetErrorLevel != null)
                return query.GetErrorLevel(value);

            return ErrorLevel.Normal;
        }
    }
}