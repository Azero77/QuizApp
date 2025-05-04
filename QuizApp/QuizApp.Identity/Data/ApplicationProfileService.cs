using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using QuizApp.Identity.Models;

namespace QuizApp.Identity.Data
{
    public class ApplicationProfileService : ProfileService<ApplicationUser>
    {
        public ApplicationProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
        {
        }

        protected override Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
        {
            return base.GetProfileDataAsync(context, user);
        }
    }
}
