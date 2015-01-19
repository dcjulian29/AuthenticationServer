using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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

        /// <summary>
        /// Returns the view containing the friendly name of the certificate used to sign tokens.
        /// </summary>
        /// <returns>The Certificate View</returns>
        public ActionResult Certificate()
        {
            var store = new X509Store("WebHosting", StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            try
            {
                var certs = store.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    Properties.Settings.Default.HostingCertThumbprint,
                    true);

                if (certs.Count == 1)
                {
                    var cert = certs[0];

                    ViewBag.CertName = cert.SubjectName.Name;
                    ViewBag.Thumbprint = cert.Thumbprint;
                }
                else
                {
                    ViewBag.CertName = "Hosting Certificate Not Found...";
                }
            }
            finally
            {
                store.Close();
            }

            return View();
        }

        /// <summary>
        /// Returns a view letting the user know they have been logged out.
        /// </summary>
        /// <returns>The Logout View</returns>
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();

            return View();
        }
    }
}