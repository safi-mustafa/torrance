using DataLibrary;
using IdentityProvider.Data.IdentityStore.ClaimHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Models;
using System.Security.Claims;

namespace IdentityProvider.Data.IdentityStore
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ToranceUser, ToranceRole>
    {
        private readonly ToranceContext _db;
        public ApplicationUserClaimsPrincipalFactory(
                                                    UserManager<ToranceUser> userManager,
                                                    RoleManager<ToranceRole> roleManager,
                                                    IOptions<IdentityOptions> optionsAccessor,
                                                    ToranceContext db
                                                    )
                                                    : base(userManager, roleManager, optionsAccessor)
        {
            _db = db;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ToranceUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            //identity.AddClaim(new Claim("Role", user.Role ?? ""));
            //identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            identity.AddClaims(await ClaimsHelper.GetClaims(user, _db));
            return identity;
        }
      
    }
}
