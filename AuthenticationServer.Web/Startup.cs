using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AuthenticationServer.Web.Config;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Models;

namespace AuthenticationServer.Web
{
    /// <summary>
    /// The startup class for the OWIN host containing the identity server service.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Creates a configuration.
        /// </summary>
        /// <param name="app">The application builder object.</param>
        public void Configuration(IAppBuilder app)
        {
            app.Map(
                "/identity",
                id => 
                {
                    id.UseIdentityServer(
                        new IdentityServerOptions
                        {
                            SiteName = "Authentication Server",
                            IssuerUri = "http://localhost",
                            SigningCertificate = LoadCertificate(),
                            Factory = InMemoryFactory.Create(Users.Get(), Clients.Get(), Scopes.Get())
                        });
                });

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                    {
                        AuthenticationType = "Cookies"
                    });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                    {
                        Authority = "https://localhost:44300/identity",
                        ClientId = "authsrv",
                        Scope = "openid profile roles",
                        RedirectUri = "https://localhost:44300/",
                        SignInAsAuthenticationType = "Cookies"
                    });
        }

        private X509Certificate2 LoadCertificate()
        {
            var pfx = String.Format(
                "{0}.Config.AuthenticationServer.pfx",
                Assembly.GetExecutingAssembly().GetName().Name);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pfx);

            Debug.Assert(stream != null, "Authentication Server Certificate Is Missing!");

            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            var cert = new X509Certificate2(bytes, String.Empty);

            return cert;
        }
    }
}