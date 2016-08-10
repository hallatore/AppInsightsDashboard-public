using System.Web.Mvc;
using System.Web.Routing;

namespace AppInsightsDashboard.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Dashboard", "{id}/{columns}", new { controller = "Dashboard", action = "Index", columns = UrlParameter.Optional });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
