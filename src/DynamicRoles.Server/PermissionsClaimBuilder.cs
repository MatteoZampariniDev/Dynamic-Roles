using DynamicRoles.Core;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace DynamicRoles.Server;
public class PermissionsClaimBuilder : PermissionsClaim
{
    public static Claim CreateClaim<TRole>(ICollection<TRole> roles)
    {
        var claimBuilder = CreateFromRoles(roles);

        return new Claim(CLAIM_NAME, claimBuilder.ToClaimValue());
    }

    private static PermissionsClaimBuilder CreateFromRoles<TRole>(ICollection<TRole> roles)
    {
        var result = new PermissionsClaimBuilder();

        foreach (var r in roles)
        {
            if (r != null)
            {
                var permissionProperties = r.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.PropertyType.IsEnum && x.PropertyType.GetCustomAttribute<PermissionsAttribute>() != null)
                    .ToArray();

                foreach (var property in permissionProperties)
                {
                    string permissionId = property.PropertyType
                        .GetCustomAttribute<PermissionsAttribute>()!.UniqueName;
                    int permissionValue = (int)(property.GetValue(r) ?? 0);

                    int currentPermissionValue = result.UserPermissions.GetValueOrDefault(permissionId);
                    result.UserPermissions[permissionId] = currentPermissionValue |= permissionValue;
                }
            }
        }

        return result;
    }
}