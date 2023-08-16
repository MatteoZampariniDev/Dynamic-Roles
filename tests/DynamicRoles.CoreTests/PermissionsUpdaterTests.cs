using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PermissionsUpdaterTests
{

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = 1)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = 4)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = 8)]

    [TestCase(1, TestPermissions.Permission_2, ExpectedResult = 3)]
    [TestCase(1, TestPermissions.Permission_3, ExpectedResult = 5)]
    [TestCase(1, TestPermissions.Permission_4, ExpectedResult = 9)]

    // 8+16
    [TestCase(24, TestPermissions.Permission_6, ExpectedResult = 56)]
    [TestCase(24, TestPermissions.Permission_7, ExpectedResult = 88)]

    [TestCase(-1, TestPermissions.Permission_3, ExpectedResult = -1)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = -1)]
    public int ShouldGrant(int currentPermissions, TestPermissions value)
    {
        return PermissionsUpdater.Grant(currentPermissions, value);
    }

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = 1)]
    [TestCase(0, TestPermissions.Permission_2, ExpectedResult = 0)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = 4)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = 8)]
    [TestCase(0, TestPermissions.Permission_5, ExpectedResult = 0)]
    [TestCase(0, TestPermissions.Permission_6, ExpectedResult = 0)]
    [TestCase(0, TestPermissions.Permission_7, ExpectedResult = 0)]
    [TestCase(0, TestPermissions.Permission_8, ExpectedResult = 0)]
    public int ShouldTryGrant(int currentPermissions, TestPermissions value)
    {
        return PermissionsUpdater.TryGrant(currentPermissions, value);
    }

    [TestCase(0, TestPermissions.Permission_2)]
    [TestCase(0, TestPermissions.Permission_5)]
    [TestCase(0, TestPermissions.Permission_6)]
    [TestCase(0, TestPermissions.Permission_7)]
    [TestCase(0, TestPermissions.Permission_8)]

    [TestCase(1, TestPermissions.Permission_5)]
    [TestCase(1, TestPermissions.Permission_6)]
    [TestCase(1, TestPermissions.Permission_7)]
    [TestCase(1, TestPermissions.Permission_8)]

    // 8+16
    [TestCase(24, TestPermissions.Permission_8)]
    public void ShouldThrowExceptionOnGrant(int currentPermissions, TestPermissions value)
    {
        FluentActions.Invoking(() => PermissionsUpdater.Grant(currentPermissions, value))
            .Should().Throw<PermissionsException>();

        FluentActions.Invoking(() => PermissionsUpdater.TryGrant(currentPermissions, value))
            .Should().NotThrow<PermissionsException>();
    }


    [TestCase(1, TestPermissions.Permission_1, ExpectedResult = 0)]

    // 1+8+16
    [TestCase(25, TestPermissions.Permission_1, ExpectedResult = 24)]
    [TestCase(25, TestPermissions.Permission_5, ExpectedResult = 9)]
    [TestCase(25, TestPermissions.Permission_4, ExpectedResult = 1)]

    [TestCase(-1, TestPermissions.Permission_1, ExpectedResult = 252)]
    [TestCase(-1, TestPermissions.Permission_4, ExpectedResult = 247)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = 127)]
    public int ShouldRevoke(int currentPermissions, TestPermissions value)
    {
        return PermissionsUpdater.Revoke(currentPermissions, value);
    }

    // 1+8+16
    [TestCase(25, TestPermissions.Permission_1, ExpectedResult = 24)]
    [TestCase(25, TestPermissions.Permission_2, ExpectedResult = 25)]
    [TestCase(25, TestPermissions.Permission_3, ExpectedResult = 25)]
    [TestCase(25, TestPermissions.Permission_4, ExpectedResult = 1)]
    [TestCase(25, TestPermissions.Permission_5, ExpectedResult = 9)]
    [TestCase(25, TestPermissions.Permission_6, ExpectedResult = 25)]
    [TestCase(25, TestPermissions.Permission_7, ExpectedResult = 25)]
    [TestCase(25, TestPermissions.Permission_8, ExpectedResult = 25)]
    public int ShouldTryRevoke(int currentPermissions, TestPermissions value)
    {
        return PermissionsUpdater.TryRevoke(currentPermissions, value);
    }

    [TestCase(0, TestPermissions.Permission_1)]
    [TestCase(0, TestPermissions.Permission_2)]

    [TestCase(1, TestPermissions.Permission_2)]

    // 8+16
    [TestCase(24, TestPermissions.Permission_6)]
    [TestCase(24, TestPermissions.Permission_8)]
    public void ShouldThrowExceptionOnRevoke(int currentPermissions, TestPermissions value)
    {
        FluentActions.Invoking(() => PermissionsUpdater.Revoke(currentPermissions, value))
            .Should().Throw<PermissionsException>();

        FluentActions.Invoking(() => PermissionsUpdater.TryRevoke(currentPermissions, value))
            .Should().NotThrow<PermissionsException>();
    }

}
