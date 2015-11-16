using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace AuthenticationServer.Web.Config
{
    /// <summary>
    /// Class containing additional scopes that will be used to determine roles for users.
    /// </summary>
    public static class Scopes
    {
        /// <summary>
        /// Gets a list of scopes
        /// </summary>
        /// <returns>A list of scopes</returns>
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
                {
                    new Scope
                        {
                            Enabled = true,
                            Name = "roles",
                            Type = ScopeType.Identity,
                            Claims = new List<ScopeClaim>
                                {
                                    new ScopeClaim("role")
                                }
                        },
                    new Scope
                        {
                            Enabled = true,
                            Name = "AssessApi",
                            Type = ScopeType.Resource
                        }
                };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}
