using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace DynamicRoles.Server.Services;
public class DefaultPermissionsUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
    where TRole : IdentityRole
    where TUser : class
{
    public DefaultPermissionsUserClaimsPrincipalFactory(UserManager<TUser> userManager,
        RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {

    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var userRoleNames = await this.UserManager.GetRolesAsync(user) ?? Array.Empty<string>();
        var userRoles = await this.RoleManager.Roles.Where(r =>
            userRoleNames.Contains(r.Name ?? string.Empty)).ToListAsync();

        identity.AddClaim(PermissionsClaimBuilder.CreateClaim(userRoles));

        return identity;
    }

}
