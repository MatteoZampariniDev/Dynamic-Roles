using DynamicRoles.Core;
using DynamicRoles.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DynamicRoles.Shared;
public static class AuthorizationExtensions
{
    public static Task<AuthorizationResult> AuthorizeAsync<TPermissionType>(this IAuthorizationService service, ClaimsPrincipal identity, TPermissionType permissions)
        where TPermissionType : struct, Enum
    {
        return service.AuthorizeAsync(identity, PolicyNameHelper.GeneratePolicyNameFor(permissions));
    }
}
