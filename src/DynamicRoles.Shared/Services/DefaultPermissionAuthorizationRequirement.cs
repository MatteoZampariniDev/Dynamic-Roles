using DynamicRoles.Core;
using Microsoft.AspNetCore.Authorization;

namespace DynamicRoles.Shared.Services;

public class DefaultPermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string UniqueName { get; }
    public int Value { get; }

    public DefaultPermissionAuthorizationRequirement(string? policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            throw new PermissionsException($"[{nameof(DefaultPermissionAuthorizationRequirement)}] PolicyName is null or empty.");
        }

        this.UniqueName = PolicyNameHelper.GetPermissionsUniqueName(policyName);
        this.Value = PolicyNameHelper.GetPermissionValueFrom(policyName);
    }
}

public class DefaultPermissionAuthorizationRequirementHandler : AuthorizationHandler<DefaultPermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultPermissionAuthorizationRequirement requirement)
    {
        var permissionClaim = context.User.FindFirst(c => c.Type == PermissionsClaim.CLAIM_NAME);

        if (permissionClaim != null && PermissionsClaim.Parse(permissionClaim.Value)
                .Validate(requirement.UniqueName, requirement.Value))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
