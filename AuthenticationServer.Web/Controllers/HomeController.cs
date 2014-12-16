using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationServer.Web.Controllers
{
    /// <summary>
    /// Represents the Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the view containing the user's claims
        /// </summary>
        /// <returns>The Home View</returns>
        [Authorize]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        /// <summary>
        /// Returns the view containing a description of this application
        /// </summary>
        /// <returns>The About View</returns>
        public ActionResult About()
        {
            return View();
        }
    }
}