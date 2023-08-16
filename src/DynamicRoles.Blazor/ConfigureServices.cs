using DynamicRoles.Blazor.Services;
using DynamicRoles.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicRoles.Blazor;
public static class ConfigureServices
{
    public static IServiceCollection AddDefaultIdentityWithDynamicRoles(this IServiceCollection services)
    {
        services.AddApiAuthorization()
            .AddAccountClaimsPrincipalFactory<DefaultClientAccountClaimsPrincipalFactory>();

        services.AddDefaultClientDynamicRoles();

        return services;
    }

    public static IServiceCollection AddDefaultClientDynamicRoles(this IServiceCollection services)
    {
        services.AddSingleton<IClientAuthorizationService, DefaultClientAuthorizationService>();
        services.AddSingleton<IAuthorizationHandler, DefaultPermissionAuthorizationRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, DefaultPermissionsAuthorizationPolicyProvider>();

        return services;
    }
}
