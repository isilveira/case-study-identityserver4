// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Authorizer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "roles",
                    Description = "Default roles",
                    UserClaims =
                    {
                        ClaimTypes.Role
                    }
                }
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource {
                    Name = "api1",
                    DisplayName = "My API #1"
                }
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client.console_one",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = {
                        "api1", "roles"
                    }
                },
                new Client
                {
                    ClientId = "client.console_user",
                    ClientName = "User credentials client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("4264F8BA-51D0-D271-E49E-E4C2E1B31744".Sha256()) },

                    AllowedScopes = { "api1" }
                },
                // MVC client using code flow + pkce
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:8100/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:8100/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:8100/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1", "roles" }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:8200/index.html",
                        "http://localhost:8200/callback.html",
                        "http://localhost:8200/silent.html",
                        "http://localhost:8200/popup.html",
                    },

                    PostLogoutRedirectUris = { "http://localhost:8200/index.html" },
                    AllowedCorsOrigins = { "http://localhost:8200" },

                    AllowedScopes = { "openid", "profile", "api1" }
                }
            };
    }
}
