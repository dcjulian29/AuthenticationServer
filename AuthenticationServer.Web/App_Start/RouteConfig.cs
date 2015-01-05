using System.Web.Mvc;
using System.Web.Routing;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// Provides methods for configuring routing.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="routes">The container holding the routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    });
        }
    }
}
