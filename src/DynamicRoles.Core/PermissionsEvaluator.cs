using System;
using System.Linq;

namespace DynamicRoles.Core
{
    public static class PermissionsEvaluator
    {
        /// <summary>
        /// Looks for the highest enabled level in a group
        /// </summary>
        public static int GetUnlockedGroupLevel<TPermissionType>(int userPermission, string groupName)
             where TPermissionType : struct, Enum
        {
            return GetUnlockedGroupLevel(PermissionsMapper.AsPermissions<TPermissionType>(userPermission), groupName);
        }

        /// <summary>
        /// Looks for the highest enabled level in a group
        /// </summary>
        public static int GetUnlockedGroupLevel<TPermissionType>(TPermissionType userPermission, string groupName)
            where TPermissionType : struct, Enum
        {
            // get permissions + group details
            var permissions = PermissionsMapper.GetPermissionsWithGroupDetails<TPermissionType>(groupName);
            // find highest level in group
            int highestLevel = permissions.Count() > 0 ? permissions.Select(x => x.Item1.Level).Max() : 0;
            int result = 0;

            if (highestLevel > 0)
            {
                for (int i = 0; i <= highestLevel; i++)
                {
                    result = i;

                    // group permissions by level
                    var permissionsInLevel = permissions.Where(x => x.Item1.Level == i).Select(x => x.Item2);

                    bool hasAnyInLevel = permissionsInLevel
                        .Any(requiredPermission => Has(userPermission, requiredPermission, true));

                    // if user doesn't have any permission in level, level found
                    if (!hasAnyInLevel)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Looks for the highest enabled level in a group
        /// </summary>
        public static int GetUnlockedPermissionGroupLevel<TSource, TPermissionType>(TSource sourceWithPermissions, string groupName)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            var (_, ownedPermission) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(sourceWithPermissions);
            return GetUnlockedGroupLevel(ownedPermission, groupName);
        }

        public static bool IsUnlocked<TPermissionType>(int providedPermission, TPermissionType permissionToCheck)
            where TPermissionType : struct, Enum
        {
            return IsUnlocked(PermissionsMapper.AsPermissions<TPermissionType>(providedPermission), permissionToCheck);
        }

        /// <summary>
        /// Check if a permission can be flagged
        /// </summary>
        /// <typeparam name="TPermissionType"></typeparam>
        /// <param name="currentPermission"></param>
        /// <param name="permissionToCheck"></param>
        /// <returns></returns>
        public static bool IsUnlocked<TPermissionType>(TPermissionType currentPermission, TPermissionType permissionToCheck)
            where TPermissionType : struct, Enum
        {
            //Get the group of the permission
            var group = PermissionsMapper.GetGroupAttribute(permissionToCheck);

            //Return true if the group level is less than or equal to the unlocked level
            return group.Level <= GetUnlockedGroupLevel(currentPermission, group.Name);
        }

        /// <summary>
        /// Get the permission property in an object and check if a permission can be flagged
        /// </summary>
        public static bool IsUnlockedPermission<TSource, TPermissionType>(TSource source, TPermissionType permission)
            where TPermissionType : struct, Enum
        {
            var (_, ownedPermission) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(source);
            return IsUnlocked(ownedPermission, permission);
        }

        public static bool Has<TPermissionType>(int providedPermission, TPermissionType requiredPermission, bool ignoreGroupLevel = false)
            where TPermissionType : struct, Enum
        {
            return Has(PermissionsMapper.AsPermissions<TPermissionType>(providedPermission), requiredPermission, ignoreGroupLevel);
        }

        /// <summary>
        /// Check if a permission value contains the given one
        /// </summary>
        public static bool Has<TPermissionType>(TPermissionType providedPermission, TPermissionType requiredPermission, bool ignoreGroupLevel = false)
            where TPermissionType : struct, Enum
        {
            bool result = providedPermission.HasFlag(requiredPermission);

            if (!ignoreGroupLevel)
            {
                result = result && IsUnlocked(providedPermission, requiredPermission);
            }

            return result;
        }

        /// <summary>
        /// Check if a permission value contains all the the given ones
        /// </summary>
        public static bool Has<TPermissionType>(TPermissionType providedPermission, bool ignoreGroupLevel = false, params TPermissionType[] requiredPermissions)
        where TPermissionType : struct, Enum
        {
            return requiredPermissions.All(required => Has(providedPermission, required, ignoreGroupLevel));
        }


        /// <summary>
        /// Get the permission property from an object and check if it contains the given one
        /// </summary>
        public static bool HasPermission<TSource, TPermissionType>(TSource sourceWithPermissions, TPermissionType requiredPermission, bool ignoreGroupLevel = false)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            var (_, ownedPermission) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(sourceWithPermissions);
            return Has(ownedPermission, requiredPermission, ignoreGroupLevel);
        }

        /// <summary>
        /// Get the permission property from an object and check if it contains the given ones
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TPermissionType"></typeparam>
        /// <param name="sourceWithPermissions"></param>
        /// <param name="ignoreGroupLevel"></param>
        /// <param name="requiredPermissions"></param>
        /// <returns></returns>
        public static bool HasPermission<TSource, TPermissionType>(TSource sourceWithPermissions, bool ignoreGroupLevel = false, params TPermissionType[] requiredPermissions)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            return requiredPermissions.All(required => HasPermission(sourceWithPermissions, required, ignoreGroupLevel));
        }

        /// <summary>
        /// Check if a permission contains all the permissions of a given type
        /// </summary>
        public static bool HasAll<TPermissionType>(int ownedPermission)
            where TPermissionType : struct, Enum
        {
            return HasAll(PermissionsMapper.AsPermissions<TPermissionType>(ownedPermission));
        }

        /// <summary>
        /// Check if a permission contains all the permissions of a given type
        /// </summary>
        public static bool HasAll<TPermissionType>(TPermissionType ownedPermission)
            where TPermissionType : struct, Enum
        {
            return PermissionsMapper.GetAll<TPermissionType>().TrueForAll(x => Has(ownedPermission, x));
        }
        /// <summary>
        /// Get the permission property from an object and check if it contains all the permissions of a given type
        /// </summary>
        public static bool HasAll<TSource, TPermissionType>(TSource source)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            var (_, ownedPermission) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(source);
            return HasAll(ownedPermission);
        }
    }
}