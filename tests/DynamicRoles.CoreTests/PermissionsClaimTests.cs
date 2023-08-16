using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PermissionsClaimTests
{
    [Test]
    public void ShouldCreateClaimValue()
    {
        var claimInstance = new PermissionsClaim(new Dictionary<string, int>
        {
            { Constants.TestPermissionsName , 1 },
        });

        string claimValue = claimInstance.ToClaimValue();

        claimValue.Should().Be($"{{\"{Constants.TestPermissionsName}\":1}}");
    }

    [Test]
    public void ShouldParseClaimValue()
    {
        string claimValue = $"{{\"{Constants.TestPermissionsName}\":1}}";
        var parsedClaimInstance = PermissionsClaim.Parse(claimValue);
        string parsedClaimValue = parsedClaimInstance.ToClaimValue();

        parsedClaimValue.Should().Be(claimValue);
    }

    [Test]
    public void ShouldValidateClaimValue()
    {
        var claimInstance = new PermissionsClaim(new Dictionary<string, int>
        {
            { Constants.TestPermissionsName, 1 },
        });

        bool validation1 = claimInstance.Validate(Constants.TestPermissionsName, 1);
        bool validation2 = claimInstance.Validate(Constants.TestPermissionsName, 2);

        validation1.Should().Be(true);
        validation2.Should().Be(false);
    }
}
