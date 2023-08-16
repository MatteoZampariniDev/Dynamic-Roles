using DynamicRoles.SharedDemo.Permissions;
using DynamicRoles.Tests;
using FluentAssertions;
using NUnit.Framework;
using System.Reflection;

namespace DynamicRoles.TestsDemo;
public class PermissionsTests
{
    [TestCase(Feature1_Permissions.None)]
    [TestCase(Feature2_Permissions.None)]
    public static void ShouldValidatePermissions<TPermissions>(TPermissions _)
     where TPermissions : struct, Enum
    {
        bool enumIsPermission = TestHelpers.EnumIsPermission<TPermissions>();
        bool permissionsGroupLevelsAreValid = TestHelpers.PermissionsGroupLevelsAreValid<TPermissions>();
        bool valuesInPermissionsAreBitwise = TestHelpers.ValuesInPermissionsAreBitwise<TPermissions>();

        enumIsPermission.Should().BeTrue();
        permissionsGroupLevelsAreValid.Should().BeTrue();
        valuesInPermissionsAreBitwise.Should().BeTrue();
    }

    [Test]
    public static void PermissionsNamesShouldBeUnique()
    {
        var permissionTypes = TestHelpers.GetPermissionsFromAssembly(Assembly.GetAssembly(typeof(Feature1_Permissions))!);

        bool result = TestHelpers.PermissionsNamesAreUnique(permissionTypes);

        result.Should().BeTrue();
    }
}
