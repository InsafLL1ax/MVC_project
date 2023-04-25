using System.Web.Mvc;
using System.Web.Routing;

namespace Laba_9
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Lesson", action = "Index" }
            );
        }
    }
}
