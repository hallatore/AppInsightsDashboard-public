using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppInsightsDashboard.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly long Hash = DateTime.UtcNow.ToFileTimeUtc() / 600000000;

        protected void Application_Start()
        {
            ServicePointManager.DefaultConnectionLimit = 32;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
