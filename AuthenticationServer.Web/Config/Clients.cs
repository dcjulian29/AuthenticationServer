using System.Collections.Generic;
using AuthenticationServer.Web.Properties;
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
                    ClientName = Settings.Default.SiteName,
                    ClientId = "authsrv",
                    Flow = Flows.Hybrid,
                    RedirectUris = new List<string>()
                        {
                            Settings.Default.SiteUri
                        }
                },
                new Client
                    {
                        Enabled = true,
                        ClientName = "Assessment Web Service",
                        ClientId = "AssessService",
                        ClientSecrets = new List<ClientSecret> { new ClientSecret("W3bS3rv!c3".Sha256()) },
                        Flow = Flows.ResourceOwner,
                        AbsoluteRefreshTokenLifetime = 3600,
                        SlidingRefreshTokenLifetime = 600,
                        RefreshTokenExpiration = TokenExpiration.Sliding
                    }
            };
        }
    }
}
