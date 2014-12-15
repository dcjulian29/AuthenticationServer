using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// This class provides access to the global events of this application.
    /// Allowing the application to do something on ApplicationStart, BeginRequest, etc.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// This code is ran when the application is started.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
