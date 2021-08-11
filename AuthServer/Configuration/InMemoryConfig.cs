using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.Configuration
{
    public static class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static List<TestUser> GetUsers() =>
            TestUsers.Users;
            //new List<TestUser>
            //{
            //    new TestUser
            //    {
            //        SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
            //        Username = "Tom",
            //        Password = "TomPassword",
            //        Claims = new List<Claim>
            //        {
            //            new Claim("given_name", "Tom"),
            //            new Claim("family_name", "Cat")
            //        }
            //    },
            //    new TestUser
            //    {
            //        SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
            //        Username = "Jerry",
            //        Password = "JerryPassword",
            //        Claims = new List<Claim>
            //        {
            //            new Claim("given_name", "Jerry"),
            //            new Claim("family_name", "Mouse")
            //        }
            //    }
            //};

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                //new Client
                //{
                //    ClientId = "customer",
                //    ClientSecrets = new [] { new Secret("TomAndJerry".Sha512()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "mvcApi" }
                //},
                new Client
                {
                    ClientName = "MVC Client",
                    ClientId = "mvc-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>{ "https://localhost:5000/signin-oidc" },
                    RequirePkce = false,
                    AllowedScopes = { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile 
                    },
                    ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                    RequireConsent = false
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("mvcApi", "MVC client API")
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("mvcApi", "MVC client API")
                {
                    Scopes = { "mvcApi" }
                }
            };
    }
}
