using DynamicRoles.Core;

namespace DynamicRoles.CoreTests.Helpers;

[Permissions(Constants.TestPermissionsName)]
public enum TestPermissions
{
    None = 0,

    [Group(Constants.TestPermissionsGroup1), Details("Permission 1", "First permission.")]
    Permission_1 = 1,
    [Group(Constants.TestPermissionsGroup1, 1)]
    Permission_2 = 2,

    [Group(Constants.TestPermissionsGroup2)]
    Permission_3 = 4,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_4 = 8,
    [Group(Constants.TestPermissionsGroup2, 1)]
    Permission_5 = 16,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_6 = 32,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_7 = 64,
    [Group(Constants.TestPermissionsGroup2, 3), Details("Permission 8", "Another permission.")]
    Permission_8 = 128,

    All = ~None
}

[Permissions(Constants.AdditionalTestPermissionsName)]
public enum AdditionalTestPermissions
{
    None = 0,

    [Group(Constants.TestPermissionsGroup1), Details("Permission 1", "First permission.")]
    Permission_1 = 1,
    [Group(Constants.TestPermissionsGroup1, 1)]
    Permission_2 = 2,

    [Group(Constants.TestPermissionsGroup2)]
    Permission_3 = 4,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_4 = 8,
    [Group(Constants.TestPermissionsGroup2, 1)]
    Permission_5 = 16,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_6 = 32,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_7 = 64,
    [Group(Constants.TestPermissionsGroup2, 3), Details("Permission 8", "Another permission.")]
    Permission_8 = 128,

    All = ~None
}

public enum FailingTestPermissions
{
    None = 0,

    [Group(Constants.TestPermissionsGroup1), Details("Permission 1", "First permission.")]
    Permission_1 = 1,
    [Group(Constants.TestPermissionsGroup1, 1)]
    Permission_2 = 2,

    [Group(Constants.TestPermissionsGroup2)]
    Permission_3 = 4,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_4 = 7,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_5 = 16,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_6 = 32,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_7 = 64,


    All = ~None
}

[Permissions(Constants.TestPermissionsName)]
public enum AdditionalFailingTestPermissions
{
    None = 0,

    [Group(Constants.TestPermissionsGroup1), Details("Permission 1", "First permission.")]
    Permission_1 = 1,
    [Group(Constants.TestPermissionsGroup1, 1)]
    Permission_2 = 2,

    [Group(Constants.TestPermissionsGroup2)]
    Permission_3 = 4,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_4 = 7,
    [Group(Constants.TestPermissionsGroup2)]
    Permission_5 = 16,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_6 = 32,
    [Group(Constants.TestPermissionsGroup2, 2)]
    Permission_7 = 64,


    All = ~None
}