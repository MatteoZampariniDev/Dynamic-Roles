using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DynamicRoles.Core
{
    public static class PermissionsMapper
    {
        /// <summary>
        /// Get all permissions of a given type
        /// </summary>
        public static List<TPermissionType> GetAll<TPermissionType>(bool includeNone = false, bool includeAll = false)
            where TPermissionType : struct, Enum
        {
            var result = Enum.GetValues(typeof(TPermissionType))
                .OfType<TPermissionType>()
                .ToList();

            if (!includeNone)
            {
                var noneValue = result.Where(x => AsInt(x) == 0).FirstOrDefault();
                result.Remove(noneValue);
            }

            if (!includeAll)
            {
                var allValue = result.Where(x => AsInt(x) == -1).FirstOrDefault();
                result.Remove(allValue);
            }

            return result;
        }

        #region Value Conversion
        /// <summary>
        /// Convert a permission to int
        /// </summary>
        public static int AsInt<TPermissionType>(this TPermissionType permissions)
            where TPermissionType : struct, Enum
        {
            return (int)(object)permissions;
        }

        /// <summary>
        /// Convert an int to permission
        /// </summary>
        public static TPermissionType AsPermissions<TPermissionType>(this int permissionsValue)
            where TPermissionType : struct, Enum
        {
            return (TPermissionType)(object)permissionsValue;
        }
        #endregion

        #region Group
        /// <summary>
        /// Get the group attribute
        /// </summary>
        public static GroupAttribute GetGroupAttribute<TPermissionType>(TPermissionType permission)
            where TPermissionType : struct, Enum
        {
            return permission.GetAttributesFrom<TPermissionType, GroupAttribute>().FirstOrDefault() ?? new GroupAttribute(string.Empty, 0);
        }

        /// <summary>
        /// Get the all the permissions in a given group
        /// </summary>
        public static IEnumerable<TPermissions> GetPermissionsInGroup<TPermissions>(string groupName)
            where TPermissions : struct, Enum
        {
            return GetAll<TPermissions>().Where(x => x.IsInGroup(groupName));
        }

        /// <summary>
        /// Get all permissions and group details
        /// </summary>
        public static IEnumerable<(GroupAttribute, TPermissionType)> GetPermissionsWithGroupDetails<TPermissionType>(string groupName)
            where TPermissionType : struct, Enum
        {
            var permissions = GetPermissionsInGroup<TPermissionType>(groupName);

            var result = new List<(GroupAttribute, TPermissionType)>();

            foreach (var p in permissions)
            {
                var group = GetGroupAttribute(p);

                if (group != null)
                {
                    result.Add((group, p));
                }
            }

            return result;
        }

        /// <summary>
        /// Check if a permission is in a given group
        /// </summary>
        public static bool IsInGroup<TPermissions>(this TPermissions permission, string groupName)
            where TPermissions : struct, Enum
        {
            var group = GetGroupAttribute(permission);
            return group != null && group.Name == groupName;
        }
        #endregion

        #region Details
        /// <summary>
        /// Get the <see cref="DetailsAttribute"/> of a permission value
        /// </summary>
        public static DetailsAttribute GetDetailsAttribute<TPermissionType>(TPermissionType permissions)
        where TPermissionType : struct, Enum
        {
            return permissions.GetAttributesFrom<TPermissionType, DetailsAttribute>().FirstOrDefault() ?? new DetailsAttribute();
        }
        /// <summary>
        /// Get the <see cref="DetailsAttribute"/> of a permission type
        /// </summary>
        public static DetailsAttribute GetDetailsAttribute<TPermissionType>()
            where TPermissionType : struct, Enum
        {
            return typeof(TPermissionType).GetCustomAttribute<DetailsAttribute>() ?? new DetailsAttribute();
        }
        #endregion

        /// <summary>
        /// Get the <see cref="PermissionsAttribute"/> of a permission type
        /// </summary>
        public static PermissionsAttribute? GetPermissionsAttribute<TPermissionType>()
            where TPermissionType : struct, Enum
        {
            return typeof(TPermissionType).GetCustomAttribute<PermissionsAttribute>();
        }

        private static IEnumerable<TAttribute> GetAttributesFrom<TPermissionType, TAttribute>(this TPermissionType permissions)
            where TPermissionType : struct, Enum
            where TAttribute : Attribute
        {
            return typeof(TPermissionType).GetMember(permissions.ToString())[0].GetCustomAttributes<TAttribute>();
        }

        /// <summary>
        /// Get the permission property and value of a given type from an object
        /// </summary>
        public static (PropertyInfo? propertyInfo, TPermissionType ownedPermission) GetPermissionFromSource<TSource, TPermissionType>(TSource source)
            where TPermissionType : struct, Enum
        {
            var property = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(TPermissionType))
                .FirstOrDefault();

            if (property == null)
            {
                return (null, AsPermissions<TPermissionType>(0));
            }
            else
            {
                var result = (TPermissionType?)property.GetValue(source);
                return (property, result ??= AsPermissions<TPermissionType>(0));
            }
        }
    }
}