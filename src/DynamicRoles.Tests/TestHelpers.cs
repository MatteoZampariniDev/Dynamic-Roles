using DynamicRoles.Core;
using System.Reflection;

namespace DynamicRoles.Tests;
public static class TestHelpers
{
    public static bool EnumIsPermission<TEnum>()
        where TEnum : struct, Enum
    {
        return typeof(TEnum).GetCustomAttribute<PermissionsAttribute>() != null;
    }

    public static bool EnumsArePermissions(params Type[] enumTypes)
    {
        return enumTypes.All(x => x.GetCustomAttribute<PermissionsAttribute>() != null);
    }

    public static bool PermissionsNamesAreUnique<TEnum>()
         where TEnum : struct, Enum
    {
        return PermissionsNamesAreUnique(typeof(TEnum));
    }

    public static bool PermissionsNamesAreUnique(params Type[] enumTypes)
    {
        var permissionsAttributes = new List<PermissionsAttribute>();

        foreach (var x in enumTypes)
        {
            var attr = x.GetCustomAttribute<PermissionsAttribute>();

            if (attr != null)
            {
                permissionsAttributes.Add(attr);
            }
        }

        return permissionsAttributes.All(x => permissionsAttributes.Where(p => p.UniqueName == x.UniqueName).Count() == 1);
    }

    public static bool PermissionsGroupLevelsAreValid<TEnum>()
        where TEnum : struct, Enum
    {
        var permissions = PermissionsMapper.GetAll<TEnum>();
        var groupAttributes = new List<GroupAttribute>();

        permissions.ForEach(x => groupAttributes.Add(PermissionsMapper.GetGroupAttribute(x)));

        var groupedPermissions = groupAttributes.GroupBy(x => x.Name);

        foreach (var group in groupedPermissions)
        {
            int maxLevel = group.Where(x => x.Level > 0).Select(x => x.Level).Max();

            for (var i = maxLevel; i > 0; i--)
            {
                if (!group.Any(x => x.Level == i - 1))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool ValuesInPermissionsAreBitwise<TEnum>()
        where TEnum : struct, Enum
    {
        var permissions = PermissionsMapper.GetAll<TEnum>();

        return permissions.All(x => IsValidBitwiseValue(PermissionsMapper.AsInt(x)));
    }

    private static bool IsValidBitwiseValue(int x)
    {
        return (x > 0) && (x == 1 || (x & (x - 1)) == 0);
    }

    public static Type[] GetPermissionsFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.IsEnum && x.GetCustomAttribute<PermissionsAttribute>() != null)
            .ToArray();
    }
}
