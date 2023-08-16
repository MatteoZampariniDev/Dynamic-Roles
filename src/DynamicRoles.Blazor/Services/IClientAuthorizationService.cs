using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DynamicRoles.Blazor.Services;
public interface IClientAuthorizationService
{
    Task<ClaimsPrincipal?> GetIdentityAsync(Task<AuthenticationState>? authState);
    Task<bool> AuthorizeAsync(Task<AuthenticationState>? authState);

    Task<bool> AuthorizeAsync<TPermissionType>(Task<AuthenticationState>? authState, TPermissionType permissions) where TPermissionType : struct, Enum;
    Task<AuthorizationResult> AuthorizeAsync<TPermissionType>(ClaimsPrincipal claims, TPermissionType permissions) where TPermissionType : struct, Enum;
    
}
