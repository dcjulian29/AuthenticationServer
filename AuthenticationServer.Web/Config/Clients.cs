using System;
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;

namespace AuthenticationServer.Web.Config
{
    /// <summary>
    /// Class containing Authorized Clients(Application) that can authenticate.
    /// </summary>
    public class Clients
    {
        /// <summary>
        /// Gets a list of authorized clients
        /// </summary>
        /// <returns>A list of authorized clients</returns>
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
