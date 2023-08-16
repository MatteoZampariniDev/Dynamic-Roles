using DynamicRoles.Core;

namespace DynamicRoles.SharedDemo.Permissions;

[Permissions(nameof(Feature2_Permissions))]
[Details("Feature 2 Permissions", "Description for permissions of feature 2.")]
public enum Feature2_Permissions
{
    None = 0,

    [Group("Group 1"), Details("View", "First permission.")]
    Permission_1 = 1,
    [Group("Group 1", 1), Details("View", "Another one for the demo.")]
    Permission_2 = 2,

    [Group("Group 2")]
    Permission_3 = 4,
    [Group("Group 2", 1)]
    Permission_4 = 8,
    [Group("Group 2", 2)]
    Permission_5 = 16,
    [Group("Group 2", 3)]
    Permission_6 = 32,
    [Group("Group 2", 3)]
    Permission_7 = 64,
    [Group("Group 2", 4)]
    Permission_8 = 128,

    All = ~None
}
