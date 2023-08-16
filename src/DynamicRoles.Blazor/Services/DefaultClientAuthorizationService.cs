using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using DynamicRoles.Shared;

namespace DynamicRoles.Blazor.Services;

public class DefaultClientAuthorizationService : IClientAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;

    public DefaultClientAuthorizationService(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<ClaimsPrincipal?> GetIdentityAsync(Task<AuthenticationState>? authState)
    {
        return authState == null ? null : (await authState).User;
    }

    public async Task<bool> AuthorizeAsync<TPermissionType>(Task<AuthenticationState>? authState, TPermissionType permissions)
        where TPermissionType : struct, Enum
    {
        var user = await this.GetIdentityAsync(authState);
        return user != null && (await this.AuthorizeAsync(user, permissions)).Succeeded;
    }

    public async Task<AuthorizationResult> AuthorizeAsync<TPermissionType>(ClaimsPrincipal claims, TPermissionType permissions)
        where TPermissionType : struct, Enum
    {
        return await _authorizationService.AuthorizeAsync(claims, permissions);
    }

    public async Task<bool> AuthorizeAsync(Task<AuthenticationState>? authState)
    {
        var user = await this.GetIdentityAsync(authState);
        return user != null && user.Identity != null && user.Identity.IsAuthenticated;
    }
}
