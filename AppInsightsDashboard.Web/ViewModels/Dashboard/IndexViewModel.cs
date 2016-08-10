using System;
using System.Collections.Generic;
using AppInsightsDashboard.Web.Business.Dashboard.Models;

namespace AppInsightsDashboard.Web.ViewModels.Dashboard
{
    public class IndexViewModel
    {
        public Guid Id { get; set; }
        public int Columns { get; set; }
        public List<DashboardItem> Items { get; set; }
    }
}