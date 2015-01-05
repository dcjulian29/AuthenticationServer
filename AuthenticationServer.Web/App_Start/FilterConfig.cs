using System.Web;
using System.Web.Mvc;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// Provides methods for configuring custom filters.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers global filters.
        /// </summary>
        /// <param name="filters">The container holding the filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
