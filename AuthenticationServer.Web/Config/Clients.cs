using System;
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;

namespace AuthenticationServer.Web.Config
{
    /// <summary>
    /// Class containing supported clients application that can authenticate.
    /// </summary>
    public class Clients
    {
        /// <summary>
        /// Gets a list of supported clients
        /// </summary>
        /// <returns>A list of supported clients</returns>
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = Properties.Settings.Default.SiteName,
                    ClientId = "authsrv",
                    Flow = Flows.Hybrid,
                    RedirectUris = new List<string>()
                        {
                            Properties.Settings.Default.SiteUri
                        }
                },
                new Client
                    {
                        Enabled = true,
                        ClientName = "Assessment Web Service",
                        ClientId = "AssessService",
                        ClientSecret = "W3bS3rv!c3",
                        Flow = Flows.ResourceOwner
                    }
            };
        }
    }
}
