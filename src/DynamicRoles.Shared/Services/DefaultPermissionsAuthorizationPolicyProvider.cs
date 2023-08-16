using DynamicRoles.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DynamicRoles.Shared.Services;
public class DefaultPermissionsAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public DefaultPermissionsAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy == null && PolicyNameHelper.IsValidPolicyName(policyName))
        {
            policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new DefaultPermissionAuthorizationRequirement(policyName))
                .Build();

            _options.AddPolicy(policyName!, policy);
        }

        return policy;
    }
}