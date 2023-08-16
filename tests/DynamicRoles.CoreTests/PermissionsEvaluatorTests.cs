using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PermissionsEvaluatorTests
{
    #region GetUnlockedGroupLevel
    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup1, ExpectedResult = 0)]
    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup2, ExpectedResult = 3)]
    public int ShouldGetUnlockedGroupLevel(TestPermissions permission, string groupName)
    {
        return PermissionsEvaluator.GetUnlockedGroupLevel(permission, groupName);
    }

    [TestCase(0, Constants.TestPermissionsGroup1, ExpectedResult = 0)]
    [TestCase(0, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    // 1+2+4
    [TestCase(7, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(7, Constants.TestPermissionsGroup2, ExpectedResult = 1)]

    //1+2+4+14
    [TestCase(23, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(23, Constants.TestPermissionsGroup2, ExpectedResult = 2)]

    [TestCase(-1, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(-1, Constants.TestPermissionsGroup2, ExpectedResult = 3)]
    public int ShouldGetUnlockedGroupLevel(int permission, string groupName)
    {
        return PermissionsEvaluator.GetUnlockedGroupLevel<TestPermissions>(permission, groupName);
    }

    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup1, ExpectedResult = 0)]
    [TestCase(TestPermissions.None, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(TestPermissions.Permission_1, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(TestPermissions.All, Constants.TestPermissionsGroup2, ExpectedResult = 3)]
    public int ShouldGetUnlockedGroupLevel_FromRole(TestPermissions permission, string groupName)
    {
        return PermissionsEvaluator.GetUnlockedPermissionGroupLevel<TestRole, TestPermissions>(new TestRole
        {
            TestPermissions = permission
        }, groupName);
    }

    [TestCase(0, Constants.TestPermissionsGroup1, ExpectedResult = 0)]
    [TestCase(0, Constants.TestPermissionsGroup2, ExpectedResult = 0)]

    // 1+2+4
    [TestCase(7, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(7, Constants.TestPermissionsGroup2, ExpectedResult = 1)]

    //1+2+4+14
    [TestCase(23, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(23, Constants.TestPermissionsGroup2, ExpectedResult = 2)]

    [TestCase(-1, Constants.TestPermissionsGroup1, ExpectedResult = 1)]
    [TestCase(-1, Constants.TestPermissionsGroup2, ExpectedResult = 3)]
    public int ShouldGetUnlockedGroupLevel_FromRole(int permission, string groupName)
    {
        return PermissionsEvaluator.GetUnlockedPermissionGroupLevel<TestRole, TestPermissions>(new TestRole
        {
            TestPermissions = PermissionsMapper.AsPermissions<TestPermissions>(permission)
        }, groupName);
    }
    #endregion

    #region IsUnlocked
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(TestPermissions.None, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(TestPermissions.All, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldBeUnlockedPermission(TestPermissions userPermission, TestPermissions permission)
    {
        return PermissionsEvaluator.IsUnlocked(userPermission, permission);
    }

    [TestCase(5, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_8, ExpectedResult = false)]

    // 1+4+16+32
    [TestCase(53, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_8, ExpectedResult = true)]

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(-1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldBeUnlockedPermission(int userPermission, TestPermissions permission)
    {
        return PermissionsEvaluator.IsUnlocked(userPermission, permission);
    }


    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(TestPermissions.Permission_1, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(TestPermissions.None, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(TestPermissions.None, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(TestPermissions.All, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(TestPermissions.All, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldBeUnlockedPermission_FromRole(TestPermissions userPermission, TestPermissions permission)
    {
        var source = new TestRole
        {
            TestPermissions = userPermission,
        };

        return PermissionsEvaluator.IsUnlockedPermission(source, permission);
    }

    [TestCase(5, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_8, ExpectedResult = false)]

    // 1+4+16+32
    [TestCase(53, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(53, TestPermissions.Permission_8, ExpectedResult = true)]

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(0, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(-1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldBeUnlockedPermission_FromRole(int userPermission, TestPermissions permission)
    {
        var source = new TestRole
        {
            TestPermissions = PermissionsMapper.AsPermissions<TestPermissions>(userPermission)
        };

        return PermissionsEvaluator.IsUnlockedPermission(source, permission);
    }
    #endregion

    #region HasPermissions
    [TestCase(1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(1, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_3, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(4, TestPermissions.Permission_1, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(4, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_8, ExpectedResult = false)]

    // 1+4
    [TestCase(5, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(-1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldHavePermissions(int userPermission, TestPermissions requiredPermission)
    {
        return PermissionsEvaluator.Has(userPermission, requiredPermission);
    }

    [TestCase(1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(1, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_3, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(1, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(4, TestPermissions.Permission_1, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(4, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(4, TestPermissions.Permission_8, ExpectedResult = false)]

    // 1+4
    [TestCase(5, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(5, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(5, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(0, TestPermissions.Permission_1, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_2, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_3, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_4, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_5, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_6, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_7, ExpectedResult = false)]
    [TestCase(0, TestPermissions.Permission_8, ExpectedResult = false)]

    [TestCase(-1, TestPermissions.Permission_1, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_2, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_3, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_4, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_5, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_6, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_7, ExpectedResult = true)]
    [TestCase(-1, TestPermissions.Permission_8, ExpectedResult = true)]
    public bool ShouldHavePermissions_FromRole(int userPermission, TestPermissions requiredPermission)
    {
        var source = new TestRole
        {
            TestPermissions = PermissionsMapper.AsPermissions<TestPermissions>(userPermission)
        };

        return PermissionsEvaluator.HasPermission(source, requiredPermission);
    }

    [TestCase(TestPermissions.All, ExpectedResult = true)]
    [TestCase(TestPermissions.None, ExpectedResult = false)]
    public bool ShouldHaveAllPermissions(TestPermissions userPermission)
    {
        return PermissionsEvaluator.HasAll(userPermission);
    }

    [TestCase(TestPermissions.All, ExpectedResult = true)]
    [TestCase(TestPermissions.None, ExpectedResult = false)]
    public bool ShouldHaveAllPermissions_FromRole(TestPermissions userPermission)
    {
        var source = new TestRole
        {
            TestPermissions = userPermission
        };

        return PermissionsEvaluator.HasAll<TestRole, TestPermissions>(source);
    }

    [TestCase(-1, ExpectedResult = true)]
    [TestCase(255, ExpectedResult = true)]
    [TestCase(0, ExpectedResult = false)]
    [TestCase(128, ExpectedResult = false)]
    public bool ShouldHaveAllPermissions(int userPermission)
    {
        return PermissionsEvaluator.HasAll<TestPermissions>(userPermission);
    }

    [TestCase(-1, ExpectedResult = true)]
    [TestCase(255, ExpectedResult = true)]
    [TestCase(0, ExpectedResult = false)]
    [TestCase(128, ExpectedResult = false)]
    public bool ShouldHaveAllPermissions_FromRole(int userPermission)
    {
        var source = new TestRole
        {
            TestPermissions = PermissionsMapper.AsPermissions<TestPermissions>(userPermission)
        };

        return PermissionsEvaluator.HasAll<TestRole, TestPermissions>(source);
    }
    #endregion
}
