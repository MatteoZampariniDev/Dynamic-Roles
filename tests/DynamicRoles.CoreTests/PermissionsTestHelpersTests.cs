using DynamicRoles.CoreTests.Helpers;
using DynamicRoles.Tests;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PermissionsTestHelpersTests
{
    [TestCase(TestPermissions.None)]
    [TestCase(AdditionalTestPermissions.None)]
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

    [TestCase(typeof(TestPermissions), typeof(AdditionalTestPermissions), ExpectedResult = true)]
    [TestCase(typeof(TestPermissions), typeof(AdditionalFailingTestPermissions), ExpectedResult = false)]
    public static bool PermissionsNamesShouldBeUnique(params Type[] permissionTypes)
    {
        return TestHelpers.PermissionsNamesAreUnique(permissionTypes);
    }


    [TestCase(FailingTestPermissions.None)]
    public static void ShouldNotValidatePermissions<TPermissions>(TPermissions permissions)
     where TPermissions : struct, Enum
    {
        bool enumIsPermission = TestHelpers.EnumIsPermission<TPermissions>();
        bool permissionsGroupLevelsAreValid = TestHelpers.PermissionsGroupLevelsAreValid<TPermissions>();
        bool valuesInPermissionsAreBitwise = TestHelpers.ValuesInPermissionsAreBitwise<TPermissions>();

        enumIsPermission.Should().BeFalse();
        permissionsGroupLevelsAreValid.Should().BeFalse();
        valuesInPermissionsAreBitwise.Should().BeFalse();
    }
}
