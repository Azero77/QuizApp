using Duende.IdentityModel;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using QuizApp.Shared;
using QuizApp.Shared.Models;
namespace QuizApp.Identity.Data
{
    public class ApplicationProfileService : ProfileService<ApplicationUser>
    {
        private readonly ILogger<ApplicationProfileService> _logger;
        public ApplicationProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ApplicationProfileService> logger) : base(userManager, claimsFactory)
        {
            _logger = logger;
        }

        protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
        {
            _logger.LogDebug("Checking claims \n\n\n\n\n");
            if (!context.IssuedClaims.Select(c => c.Type).Contains(JwtClaimTypes.Role))
            {
                context.IssuedClaims.Add(new System.Security.Claims.Claim(JwtClaimTypes.Role,ApplicationConstants.Roles.User));
            }
            await base.GetProfileDataAsync(context, user);
        }
    }
}
