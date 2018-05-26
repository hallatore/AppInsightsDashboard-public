using System;
using System.Threading.Tasks;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards.DashboardCustom_Queries
{
    /// <summary>
    /// Example of custom query. Could be Ping, Google analytics, etc...
    /// </summary>
    public class HelloWorldQuery : ICustomQuery
    {
        public HelloWorldQuery(string name)
        {
            Name = name;
            Format = "{0:0}";
            Postfix = "";
            GetErrorLevel = (v) => ErrorLevel.Error;
        }

        public string Name { get; }
        public string Format { get; }
        public string Postfix { get; }
        public Func<double?, ErrorLevel> GetErrorLevel { get; }

        public Task<double?> GetStatus()
        {
            return Task.FromResult<double?>(42);
        }
    }
}