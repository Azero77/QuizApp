using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;
using QuizApp.Shared;
using Serilog;
using static System.Net.WebRequestMethods;

namespace QuizApp.BFF
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

            builder.Services.AddControllers();

            // add BFF services and server-side session management
            builder.Services.AddBff()
                .AddRemoteApis()
                .AddServerSideSessions();
            builder.Services.AddCors(o => 
            {
                o.AddPolicy("SPAClients",p =>
                {
                    p.WithOrigins(GlobalConfig.ClientUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultSignOutScheme = "oidc";
                })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "__Host-bff";
                    options.Cookie.SameSite = SameSiteMode.Strict;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = GlobalConfig.IdentityUrl;
                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("exam.read");
                    options.Scope.Add("exam.write");
                    options.Scope.Add("submission.readown");
                    options.Scope.Add("submission.write");
                    options.Scope.Add("submission.readall");
                    options.Scope.Add("examgenerator.read");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("role");
                    options.ClaimActions.MapUniqueJsonKey("role","role");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";

                });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("SPAClients");
            app.UseAuthentication();

            // add CSRF protection and status code handling for API endpoints
            app.UseBff();
            app.UseAuthorization();
            app.MapBffManagementEndpoints();


            app.MapRemoteBffApiEndpoint("/api", $"{GlobalConfig.APIUrl}/api")
               .WithOptionalUserAccessToken();
            app.MapGet("/", (context) => {
                context.Response.Redirect(GlobalConfig.ClientUrl);
                return Task.CompletedTask;
            });

            app.MapGet("/accesstoken", async (HttpContext context) =>
            {
                return await context.GetUserAccessTokenAsync();
            });
            return app;
        }
    }
}
