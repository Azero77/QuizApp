using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Identity.Data;
using QuizApp.Identity.Models;
using Serilog;
using System.Security.Claims;

namespace QuizApp.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (roleManager.Roles.Count() == 0)
                {
                    roleManager.CreateAsync(new IdentityRole(ApplicationConstants.Roles.Admin)).Wait();
                    roleManager.CreateAsync(new IdentityRole(ApplicationConstants.Roles.User)).Wait();
                    Log.Debug("Roles Added");
                }
                var anas = userMgr.FindByNameAsync("Anas").Result;
                if (anas == null)
                {
                    anas = new ApplicationUser
                    {
                        UserName = "Anas",
                        Email = "Anas@example.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(anas, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(anas, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "anas Smith"),
                                new Claim(JwtClaimTypes.GivenName, "anas"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.WebSite, "http://anas.example.com"),
                            }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("anas created");
                }
                else
                {
                    Log.Debug("anas already exists");
                    if (userMgr.GetRolesAsync(anas).Result.Count == 0)
                    {
                        userMgr.AddToRoleAsync(anas, ApplicationConstants.Roles.Admin).Wait();
                        Log.Debug("Roles Added To anas");
                    }
                }

                var asaad = userMgr.FindByNameAsync("Asaad").Result;
                if (asaad == null)
                {
                    asaad = new ApplicationUser
                    {
                        UserName = "Asaad",
                        Email = "Asaad@example.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(asaad, "Pass123$").Result;
                    userMgr.AddToRoleAsync(asaad, ApplicationConstants.Roles.Admin).Wait();
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(asaad, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "asaad Smith"),
                                new Claim(JwtClaimTypes.GivenName, "asaad"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.WebSite, "http://asaad.example.com"),
                            }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("asaad created");
                }
                else
                {
                    if (userMgr.GetRolesAsync(asaad).Result.Count == 0)
                    {
                        userMgr.AddToRoleAsync(asaad, ApplicationConstants.Roles.Admin);
                        Log.Debug("Roles Added To asaad");
                    }
                    Log.Debug("asad already exists");
                }
            }
        }
    }
}
