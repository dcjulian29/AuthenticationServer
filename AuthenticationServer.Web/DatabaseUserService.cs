using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationServer.Web.Properties;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// Provide a custom User Service for this Authentication Server to use
    /// the username and password database to authenticate users
    /// </summary>
    public class DatabaseUserService : IUserService
    {
        private Claim _databaseClaim = new Claim("role", "AuthDatabase");
        private readonly static ILog _log = LogProvider.GetCurrentClassLogger();

        /// <summary>
        /// This method gets called when the user uses an external identity provider to authenticate.
        /// </summary>
        /// <param name="externalUser">The external user.</param>
        /// <param name="message">The sign-in message.</param>
        /// <returns>The authentication result.</returns>
        public Task<AuthenticateResult> AuthenticateExternalAsync(
            ExternalIdentity externalUser,
            SignInMessage message = null)
        {
            return Task.FromResult<AuthenticateResult>(null);
        }

        /// <summary>
        /// This methods gets called for when the user uses the username and password dialog.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="message">The sign-in message.</param>
        /// <returns>The authentication result</returns>
        public Task<AuthenticateResult> AuthenticateLocalAsync(
            string username,
            string password,
            SignInMessage message = null)
        {
            _log.Debug("User Database Connection String")
            _log.Debug(Settings.Default.UserDatabase)
            _log.Debug("User Query")
            _log.Debug(Settings.Default.UserQuery)
            
            using (var connection = new SqlConnection(Settings.Default.UserDatabase))
            {
                connection.Open();

                var command = new SqlCommand(Settings.Default.UserQuery, connection);

                var userParameter = new SqlParameter
                {
                    ParameterName = "@USER",
                    Value = username
                };

                var passwordParameter = new SqlParameter
                {
                    ParameterName = "@PASS",
                    Value = password
                };

                command.Parameters.Add(userParameter);
                command.Parameters.Add(passwordParameter);

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    connection.Close();
                    return Task.FromResult<AuthenticateResult>(null);
                }

                reader.Read();

                var nameIdentifier = (string)reader[0];
                
                var result = new AuthenticateResult(
                    nameIdentifier,
                    username,
                    new List<Claim> { _databaseClaim },
                    "Database");

                connection.Close();

                return Task.FromResult<AuthenticateResult>(result);
            }
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="requestedClaimTypes">The requested claim types.</param>
        /// <returns>
        /// The Claims for the user
        /// </returns>
        public Task<IEnumerable<Claim>> GetProfileDataAsync(
            ClaimsPrincipal subject,
            IEnumerable<string> requestedClaimTypes = null)
        {
            return Task.FromResult<IEnumerable<Claim>>(new List<Claim> { _databaseClaim });
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user
        /// is valid or active (e.g. during token issuance or validation)
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns><c>true</c> that the user is active</returns>
        public Task<bool> IsActiveAsync(ClaimsPrincipal subject)
        {
            return Task.FromResult(true);
        }
        
        /// <summary>
        /// This methods gets called before the login page is shown. This allows you to authenticate the user
        /// somehow based on data coming from the host (e.g. client certificates or trusted headers)
        /// </summary>
        /// <param name="message">The sign-in message.</param>
        /// <returns>The authentication result or null to continue the flow.</returns>
        public Task<AuthenticateResult> PreAuthenticateAsync(SignInMessage message)
        {
            return Task.FromResult<AuthenticateResult>(null);
        }

        /// <summary>
        /// This method gets called when the user signs out (allows to cleanup resources)
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>A "null" result</returns>
        public Task SignOutAsync(ClaimsPrincipal subject)
        {
            return Task.FromResult(0);
        }
    }
}