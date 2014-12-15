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
                    ClientName = "AuthorizationServer",
                    ClientId = "authsrv",
                    Flow = Flows.Hybrid,
                    RedirectUris = new List<string>()
                        {
                            "https://localhost:44300/"
                        }
                }
            };
        }
    }
}
