﻿using System;
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
                            SiteName = Properties.Settings.Default.SiteName,
                            IssuerUri = Properties.Settings.Default.IssuerUri,
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
                        Authority = String.Format("{0}/identity", Properties.Settings.Default.SiteUri),
                        ClientId = "authsrv",
                        Scope = "openid profile roles",
                        RedirectUri = Properties.Settings.Default.SiteUri,
                        SignInAsAuthenticationType = "Cookies"
                    });
        }

        private X509Certificate2 LoadCertificate()
        {
            X509Certificate2 cert = null;

            if (Properties.Settings.Default.IssuerUri.Contains("localhost"))
            {
                var pfx = String.Format(
                    "{0}.Config.AuthenticationServer.pfx",
                    Assembly.GetExecutingAssembly().GetName().Name);
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pfx);

                Debug.Assert(stream != null, "Authentication Server Certificate Is Missing!");

                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                cert = new X509Certificate2(bytes, String.Empty);
            }
            else
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
                        cert = certs[0];
                    }
                }
                finally
                {
                    store.Close();
                }
            }

            Debug.Assert(cert != null, "Signing Certificate is Missing!");
            return cert;
        }
    }
}