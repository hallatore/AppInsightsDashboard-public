using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppInsightsDashboard.Web.Business.Dashboard.Models;
using AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards;

namespace AppInsightsDashboard.Web.Business.Dashboard
{
    public class DashboardHelper
    {
        private readonly Config _config;

        public DashboardHelper(Config config)
        {
            _config = config;
        }

        public Dictionary<string, IDashboard> GetItems(Guid id)
        {
            if (_config.Dashboards.ContainsKey(id))
                return _config.Dashboards[id];

            return new Dictionary<string, IDashboard>();
        }

        public async Task<IItemStatus> GetStatus(Guid id, string name)
        {
            return await _config.Dashboards[id][name].GetStatus();
        }
    }
}