using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PermissionsMapperTests
{
    [Test]
    public void ShouldGetAll()
    {
        var resultWithAllAndNone = PermissionsMapper.GetAll<TestPermissions>(true, true);

        var resultWithoutAllAndNone = PermissionsMapper.GetAll<TestPermissions>();

        resultWithAllAndNone.Should().NotBeEmpty();
        resultWithAllAndNone.Should().Contain(TestPermissions.None);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_1);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_2);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_3);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_4);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_5);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_6);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_7);
        resultWithAllAndNone.Should().Contain(TestPermissions.Permission_8);
        resultWithAllAndNone.Should().Contain(TestPermissions.All);

        resultWithoutAllAndNone.Should().NotBeEmpty();
        resultWithoutAllAndNone.Should().NotContain(TestPermissions.None);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_1);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_2);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_3);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_4);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_5);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_6);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_7);
        resultWithoutAllAndNone.Should().Contain(TestPermissions.Permission_8);
        resultWithoutAllAndNone.Should().NotContain(TestPermissions.All);
    }

    [TestCase(TestPermissions.None, ExpectedResult = 0)]
    [TestCase(TestPermissions.Permission_1, ExpectedResult = 1)]
    [TestCase(TestPermissions.Permission_8, ExpectedResult = 128)]
    [TestCase(TestPermissions.All, ExpectedResult = -1)]
    public int ShouldConvertToInt(TestPermissions permission)
    {
        return PermissionsMapper.AsInt(permission);
    }

    [TestCase(0, ExpectedResult = TestPermissions.None)]
    [TestCase(1, ExpectedResult = TestPermissions.Permission_1)]
    [TestCase(128, ExpectedResult = TestPermissions.Permission_8)]
    [TestCase(-1, ExpectedResult = TestPermissions.All)]
    public TestPermissions ShouldConvertToPermission(int value)
    {
        return PermissionsMapper.AsPermissions<TestPermissions>(value);
    }

    [TestCase(TestPermissions.None, "", 0)]
    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup1, 0)]
    [TestCase(TestPermissions.Permission_2, Constants.TestPermissionsGroup1, 1)]
    [TestCase(TestPermissions.Permission_3, Constants.TestPermissionsGroup2, 0)]
    [TestCase(TestPermissions.Permission_4, Constants.TestPermissionsGroup2, 0)]
    [TestCase(TestPermissions.Permission_5, Constants.TestPermissionsGroup2, 1)]
    [TestCase(TestPermissions.Permission_6, Constants.TestPermissionsGroup2, 2)]
    [TestCase(TestPermissions.Permission_7, Constants.TestPermissionsGroup2, 2)]
    [TestCase(TestPermissions.Permission_8, Constants.TestPermissionsGroup2, 3)]
    [TestCase(TestPermissions.All, "", 0)]
    public void ShouldGetGroup(TestPermissions permission, string name, int level)
    {
        var group = PermissionsMapper.GetGroupAttribute(permission);

        group.Should().NotBeNull();
        group.Name.Should().Be(name);
        group.Level.Should().Be(level);
    }

    [Test]
    public void GetPermissionsInGroup()
    {
        var permissions = PermissionsMapper
            .GetPermissionsInGroup<TestPermissions>(Constants.TestPermissionsGroup2);

        permissions.Should().NotBeEmpty();
        permissions.Should().NotContain(TestPermissions.None);
        permissions.Should().NotContain(TestPermissions.Permission_1);
        permissions.Should().NotContain(TestPermissions.Permission_2);
        permissions.Should().Contain(TestPermissions.Permission_3);
        permissions.Should().Contain(TestPermissions.Permission_4);
        permissions.Should().Contain(TestPermissions.Permission_5);
        permissions.Should().Contain(TestPermissions.Permission_6);
        permissions.Should().Contain(TestPermissions.Permission_7);
        permissions.Should().Contain(TestPermissions.Permission_8);
        permissions.Should().NotContain(TestPermissions.All);
    }

    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup1, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_2, Constants.TestPermissionsGroup1, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_3, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_4, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_5, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_6, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_7, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_8, Constants.TestPermissionsGroup1, ExpectedResult = false)]
    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup1, ExpectedResult = false)]

    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup2, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup2, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_2, Constants.TestPermissionsGroup2, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_3, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_4, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_5, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_6, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_7, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_8, Constants.TestPermissionsGroup2, ExpectedResult = true)]
    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup2, ExpectedResult = false)]
    public bool ShouldBeInGroup(TestPermissions permission, string groupName)
    {
        return PermissionsMapper.IsInGroup(permission, groupName);
    }

    [TestCase(TestPermissions.None, "", "")]
    [TestCase(TestPermissions.Permission_1, "Permission 1", "First permission.")]
    [TestCase(TestPermissions.Permission_2, "", "")]
    [TestCase(TestPermissions.Permission_3, "", "")]
    [TestCase(TestPermissions.Permission_4, "", "")]
    [TestCase(TestPermissions.Permission_5, "", "")]
    [TestCase(TestPermissions.Permission_6, "", "")]
    [TestCase(TestPermissions.Permission_7, "", "")]
    [TestCase(TestPermissions.Permission_8, "Permission 8", "Another permission.")]
    [TestCase(TestPermissions.All, "", "")]
    public void ShouldGetDetails(TestPermissions permission, string name, string description)
    {
        var details = PermissionsMapper.GetDetailsAttribute(permission);

        details.Should().NotBeNull();
        details.Name.Should().Be(name);
        details.Description.Should().Be(description);
    }

    public void ShouldGetPermissionsAttribute()
    {
        var attr = PermissionsMapper.GetPermissionsAttribute<TestPermissions>();

        attr.Should().NotBeNull();
        attr!.UniqueName.Should().Be(Constants.TestPermissionsName);
    }

    public void ShouldGetPermissionsFromSource()
    {
        var role = new TestRole
        {
            TestPermissions = TestPermissions.Permission_1
        };

        var result = PermissionsMapper.GetPermissionFromSource<TestRole, TestPermissions>(role);

        result.Should().NotBeNull();
        result.propertyInfo.Should().NotBeNull();
        result.ownedPermission.Should().Be(TestPermissions.Permission_1);
    }
}
