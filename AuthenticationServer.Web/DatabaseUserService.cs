using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationServer.Web.Properties;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// Provide a custom User Service for this Authentication Server to use the username and
    /// password database to authenticate users
    /// </summary>
    public class DatabaseUserService : IUserService
    {
        private Claim _databaseClaim = new Claim("role", "AuthDatabase");
        private ILog _log = LogProvider.GetCurrentClassLogger();

        /// <summary>
        /// This method gets called when the user uses an external identity provider to
        /// authenticate. The user's identity from the external provider is passed via the
        /// `externalUser` parameter which contains the provider identifier, the provider's
        /// identifier for the user, and the claims from the provider for the external user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A "null" result</returns>
        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called for local authentication (whenever the user uses the username
        /// and password dialog).
        /// </summary>
        /// <param name="context">The authentication context.</param>
        /// <returns>A "null" result</returns>
        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            _log.Debug("User Database Connection String");
            _log.Debug(Settings.Default.UserDatabase);
            _log.Debug("User Query");
            _log.Debug(Settings.Default.UserQuery);

            using (var connection = new SqlConnection(Settings.Default.UserDatabase))
            {
                connection.Open();

                var command = new SqlCommand(Settings.Default.UserQuery, connection);

                var userParameter = new SqlParameter
                {
                    ParameterName = "@USER",
                    Value = context.UserName
                };

                var passwordParameter = new SqlParameter
                {
                    ParameterName = "@PASS",
                    Value = context.Password
                };

                command.Parameters.Add(userParameter);
                command.Parameters.Add(passwordParameter);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    var nameIdentifier = (string)reader[0];

                    context.AuthenticateResult = new AuthenticateResult(
                        nameIdentifier,
                        context.UserName,
                        new List<Claim> { _databaseClaim },
                        "Database");
                }

                connection.Close();

                return Task.FromResult(0);
            }
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token
        /// creation or via the user info endpoint)
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>A "null" result</returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims = new List<Claim>
            {
                _databaseClaim
            };

            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid
        /// or active (e.g. if the user's account has been deactivated since they logged in). (e.g.
        /// during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A "null" result</returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }

        /// <summary>
        /// This method is called prior to the user being issued a login cookie for IdentityServer.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A "null" result</returns>
        public Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called before the login page is shown. This allows you to determine if
        /// the user should be authenticated by some out of band mechanism (e.g. client certificates
        /// or trusted headers).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A "null" result</returns>
        public Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called when the user signs out.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A "null" result</returns>
        public Task SignOutAsync(SignOutContext context)
        {
            return Task.FromResult(0);
        }
    }
}
