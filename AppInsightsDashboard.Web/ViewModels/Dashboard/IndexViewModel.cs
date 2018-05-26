using System;
using System.Collections.Generic;
using AppInsightsDashboard.Web.Business.Dashboard.Models;
using AppInsightsDashboard.Web.Business.Dashboard.Models.Dashboards;

namespace AppInsightsDashboard.Web.ViewModels.Dashboard
{
    public class IndexViewModel
    {
        public Guid Id { get; set; }
        public int Columns { get; set; }
        public Dictionary<string, IDashboard> Items { get; set; }
    }
}