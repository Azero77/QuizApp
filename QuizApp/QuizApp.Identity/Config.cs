using Duende.IdentityModel;
using Duende.IdentityServer.Models;

namespace QuizApp.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(JwtClaimTypes.Roles,new string[] { JwtClaimTypes.Roles })
            };

        public static IEnumerable<ApiScope> ApiScopes => new[]
            {
                new ApiScope("exam.read", "Read exams"),
                new ApiScope("exam.write", "Write exams"),
                new ApiScope("submission.write", "Create submissions"),
                new ApiScope("submission.readown", "Read user's submissions"),
                new ApiScope("submission.readall", "Read all submissions"),
                new ApiScope("examgenerator.read", "Read exam generator")
            };


        public static IEnumerable<ApiResource> ApiResources => new [] {
                new ApiResource("exams","Display And Correct Exams")
                {
                   Scopes = { "exam.read","exam.write"},
                   UserClaims = { JwtClaimTypes.Role }
                },
                new ApiResource("submissions","Read And Add Submissions")
                {
                    Scopes = { "submission.write","submission.readown","submission.readall"}, //readall -> read all submissions - readown -> read only user's submission
                    UserClaims = {JwtClaimTypes.Role }
                },
                new ApiResource("examgenerator","Generating your own submission"){
                    Scopes = { "examgenerator.read"},
                    UserClaims = {JwtClaimTypes.Role } //in the future everyone can generate exam so no need for role
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },
                new Client
                {
                    ClientId = "interactive.confidential",
                    ClientSecrets = {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email" ,
                        "exam.read",
                        "exam.write", //admin
                        "submission.readown", 
                        "submission.readall",//admin
                        "submission.write", //admin
                        "examgenerator.read", //admin
                    }

                }
            };
    }
}
