using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using DynamicRoles.Server;
using DynamicRoles.Shared;
using DynamicRoles.Shared.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using System.Security.Claims;

namespace DynamicRoles.ServerTests;
public class AuthorizeTests
{
    [TestCase(0, TestPermissions.Permission_1, false)]
    [TestCase(0, TestPermissions.Permission_1 | TestPermissions.Permission_3, false)]

    [TestCase(1, TestPermissions.Permission_1, true)]
    [TestCase(1, TestPermissions.Permission_1 | TestPermissions.Permission_3, true)]

    [TestCase(4, TestPermissions.Permission_1 | TestPermissions.Permission_3, true)]

    // 1+4
    [TestCase(5, TestPermissions.Permission_1, true)]
    [TestCase(5, TestPermissions.Permission_3, true)]
    [TestCase(5, TestPermissions.Permission_1 | TestPermissions.Permission_3, true)]

    [TestCase(-1, TestPermissions.Permission_7, true)]
    public async Task ShouldAuthorizeWithPermissions(int userPermissions, TestPermissions permissionRequired, bool succeed)
    {
        var userClaimPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim> 
                { 
                    PermissionsClaimBuilder.CreateClaim(
                        new List<TestRole>
                        {
                            new TestRole
                            {
                                TestPermissions = PermissionsMapper.AsPermissions<TestPermissions>(userPermissions)
                            }
                        })
                })
            );

        var authAttr = new AuthorizeAttribute<TestPermissions>(permissionRequired);

        var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new DefaultPermissionAuthorizationRequirement(authAttr.Policy)
        }, userClaimPrincipal, null);

        var requirementHandler = new DefaultPermissionAuthorizationRequirementHandler();
        await requirementHandler.HandleAsync(context);

        if (succeed)
        {
            context.HasSucceeded.Should().BeTrue();
        }
        else
        {
            context.HasSucceeded.Should().BeFalse();
        }
    }
}
