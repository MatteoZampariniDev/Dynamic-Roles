using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Interfaces;
using DynamicRoles.Server.Services;
using DynamicRoles.Shared.Services;

namespace DynamicRoles.Server;
public static class ConfigureServices
{
    public static IServiceCollection AddDefaultIdentityWithDynamicRoles<TUser, TRole, TDbContext>(this IServiceCollection services)
        where TUser : class
        where TRole : IdentityRole
        where TDbContext : DbContext, IPersistedGrantDbContext
    {
        return services.AddDefaultIdentityWithDynamicRoles<TUser, TRole, TDbContext>(null);
    }

    public static IServiceCollection AddDefaultIdentityWithDynamicRoles<TUser, TRole, TDbContext>(this IServiceCollection services, Action<IdentityOptions>? configureOptions)
        where TUser : class
        where TRole : IdentityRole
        where TDbContext : DbContext, IPersistedGrantDbContext
    {
        if (configureOptions != null)
        {
            services.AddDefaultIdentity<TUser>(configureOptions)
                .AddRoles<TRole>()
                .AddEntityFrameworkStores<TDbContext>()
                .AddClaimsPrincipalFactory<DefaultPermissionsUserClaimsPrincipalFactory<TUser, TRole>>();
        }
        else
        {
            services.AddDefaultIdentity<TUser>()
                .AddRoles<TRole>()
                .AddEntityFrameworkStores<TDbContext>()
                .AddClaimsPrincipalFactory<DefaultPermissionsUserClaimsPrincipalFactory<TUser, TRole>>();
        }

        services.AddIdentityServer()
            .AddApiAuthorization<TUser, TDbContext>(options =>
            {
                options.IdentityResources["openid"].UserClaims.Add("role");
                options.ApiResources.Single().UserClaims.Add("role");
                options.IdentityResources["openid"].UserClaims.Add("permissions");
                options.ApiResources.Single().UserClaims.Add("permissions");
            });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddDefaultServerDynamicRoles();

        return services;
    }

    public static IServiceCollection AddDefaultServerDynamicRoles(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, DefaultPermissionAuthorizationRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, DefaultPermissionsAuthorizationPolicyProvider>();

        return services;
    }
}
