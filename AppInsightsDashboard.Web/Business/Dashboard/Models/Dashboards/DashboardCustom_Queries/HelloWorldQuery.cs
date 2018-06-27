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
            Type = "Value";
            Name = name;
            Postfix = "";
            GetErrorLevel = (v) => ErrorLevel.Error;
        }
        
        public string Type { get; }
        public string Name { get; }
        public Func<dynamic, dynamic> Format => (v) => string.Format("{0:0}", v);
        public string Postfix { get; }
        public Func<dynamic, ErrorLevel> GetErrorLevel { get; }

        public Task<dynamic> GetStatus()
        {
            return Task.FromResult<dynamic>(42);
        }
    }
}