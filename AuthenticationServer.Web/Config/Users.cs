using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace AuthenticationServer.Web.Config
{
    /// <summary>
    /// Class containing users that can authenticate
    /// </summary>
    /// <remarks>Currently contains "test" users.</remarks>
    public static class Users
    {
        /// <summary>
        /// Gets a list of authorized users.
        /// </summary>
        /// <returns>A list of authorized users</returns>
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
                {
                    new InMemoryUser
                        {
                            Username = "coordinator1",
                            Password = "Password1",
                            Subject = "1",
                            Claims = new[]
                                {
                                    new Claim(Constants.ClaimTypes.GivenName, "Coodinator"),
                                    new Claim(Constants.ClaimTypes.FamilyName, "One") 
                                }
                        },
                    new InMemoryUser
                        {
                            Username = "coordinator2",
                            Password = "Password2",
                            Subject = "1",
                            Claims = new[]
                                {
                                    new Claim(Constants.ClaimTypes.GivenName, "Coodinator"),
                                    new Claim(Constants.ClaimTypes.FamilyName, "Two") 
                                }
                        },
                    new InMemoryUser
                        {
                            Username = "admin",
                            Password = "Pa$$w0rd",
                            Subject = "1",
                            Claims = new[]
                                {
                                    new Claim(Constants.ClaimTypes.GivenName, "System"),
                                    new Claim(Constants.ClaimTypes.FamilyName, "Administrator") 
                                }
                        }
                };
        }
    }
}
