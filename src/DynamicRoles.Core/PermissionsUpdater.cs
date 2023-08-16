using System;
using System.Reflection;

namespace DynamicRoles.Core
{
    public static class PermissionsUpdater
    {
        /// <summary>
        /// Grant or revoke a permission
        /// </summary>
        public static int TryGrantOrRevoke<TPermissionType>(int currentPermission, TPermissionType value, bool grant = true)
            where TPermissionType : struct, Enum
        {
            return grant ? TryGrant(currentPermission, value) : TryRevoke(currentPermission, value);
        }

        /// <summary>
        /// Grant or revoke a permission
        /// </summary>
        public static TPermissionType TryGrantOrRevoke<TPermissionType>(TPermissionType currentPermission, TPermissionType value, bool grant = true)
            where TPermissionType : struct, Enum
        {
            return grant ? TryGrant(currentPermission, value) : TryRevoke(currentPermission, value);
        }

        /// <summary>
        /// Grant or revoke a permission
        /// </summary>
        public static int GrantOrRevoke<TPermissionType>(int currentPermission, TPermissionType value, bool grant = true)
            where TPermissionType : struct, Enum
        {
            return grant ? Grant(currentPermission, value) : Revoke(currentPermission, value);
        }

        /// <summary>
        /// Grant or revoke a permission
        /// </summary>
        public static TPermissionType GrantOrRevoke<TPermissionType>(TPermissionType currentPermission, TPermissionType value, bool grant = true)
            where TPermissionType : struct, Enum
        {
            return grant ? Grant(currentPermission, value) : Revoke(currentPermission, value);
        }

        /// <summary>
        /// Grant or revoke a permission
        /// </summary>
        public static void GrantOrRevoke<TSource, TPermissionType>(TSource source, TPermissionType value, bool grant = true)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            (PropertyInfo? permissionProperty, TPermissionType currentPermission) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(source);

            if (permissionProperty != null)
            {
                currentPermission = GrantOrRevoke(currentPermission, value, grant);
                permissionProperty.SetValue(source, currentPermission);
            }
            else
            {
                throw new PermissionsException($"[{nameof(PermissionsUpdater)}] No property of type {typeof(TPermissionType)} found in {typeof(TSource)}");
            }
        }

        /// <summary>
        /// Grant or revoke all permissions of a given type
        /// </summary>
        public static int GrantOrRevokeAll(bool grant = true)
        {
            return grant ? -1 : 0;
        }

        public static TPermissionType GrantOrRevokeAll<TPermissionType>(bool grant = true)
            where TPermissionType : struct, Enum
        {
            return GrantOrRevokeAll(grant).AsPermissions<TPermissionType>();
        }

        /// <summary>
        /// Grant or revoke all permissions of a given type
        /// </summary>
        public static void GrantOrRevokeAll<TSource, TPermissionType>(TSource source, bool grant = true)
            where TPermissionType : struct, Enum
            where TSource : class
        {
            (PropertyInfo? permissionProperty, _) = PermissionsMapper.GetPermissionFromSource<TSource, TPermissionType>(source);

            if (permissionProperty != null)
            {
                TPermissionType currentPermission = GrantOrRevokeAll<TPermissionType>(grant);
                permissionProperty.SetValue(source, currentPermission);
            }
            else
            {
                throw new PermissionsException($"[{nameof(PermissionsUpdater)}] No property of type {typeof(TPermissionType)} found in {typeof(TSource)}");
            }
        }

        /// <summary>
        /// Grant a permission
        /// </summary>
        public static int TryGrant<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            try
            {
                return Grant(currentPermission, value);
            }
            catch
            {
                return currentPermission;
            }
        }

        /// <summary>
        /// Grant a permission
        /// </summary>
        public static TPermissionType TryGrant<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            try
            {
                return Grant(currentPermission, value);
            }
            catch
            {
                return currentPermission;
            }
        }

        /// <summary>
        /// Grant a permission
        /// </summary>
        public static int Grant<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            // preliminary checks
            if (currentPermission == -1)
            {
                return -1;
            }

            if (!PermissionsEvaluator.IsUnlocked(currentPermission, value))
            {
                throw new PermissionsException($"[{nameof(PermissionsEvaluator)}] Permission {value} ({PermissionsMapper.AsInt(value)} cannot be granted because its level is not unlocked (current permission: {currentPermission}).");
            }

            // update value
            currentPermission |= PermissionsMapper.AsInt(value);

            // if has all permissions override it with -1 (-1 = All)
            if (PermissionsEvaluator.HasAll<TPermissionType>(currentPermission))
            {
                currentPermission = -1;
            }

            return currentPermission;
        }

        /// <summary>
        /// Grant a permission
        /// </summary>
        public static TPermissionType Grant<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            return Grant(currentPermission.AsInt(), value).AsPermissions<TPermissionType>();
        }

        /// <summary>
        /// Revoke a permission
        /// </summary>
        public static int TryRevoke<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            try
            {
                return Revoke(currentPermission, value);
            }
            catch
            {
                return currentPermission;
            }
        }

        /// <summary>
        /// Revoke a permission
        /// </summary>
        public static TPermissionType TryRevoke<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            try
            {
                return Revoke(currentPermission, value);
            }
            catch
            {
                return currentPermission;
            }
        }


        /// <summary>
        /// Revoke a permission
        /// </summary>
        public static int Revoke<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            // preliminary checks
            if (!PermissionsEvaluator.Has(currentPermission, value))
            {
                throw new PermissionsException($"[{nameof(PermissionsEvaluator)}] Permission {value} ({PermissionsMapper.AsInt(value)} cannot be revoked because it is not granted at the moment (current permission: {currentPermission}).");
            }

            // if has -1 = All, clear them and add each of them individually
            if (currentPermission == -1)
            {
                currentPermission = 0;

                var allPermissions = PermissionsMapper.GetAll<TPermissionType>();

                foreach (var p in allPermissions)
                {
                    currentPermission = AddFlag(currentPermission, p);
                }
            }

            // remove the flag
            var updatedPermission = RemoveFlag(currentPermission, value);

            // remove all flags of permissions that are locked by their level
            var group = PermissionsMapper.GetGroupAttribute(value);

            if (group != null)
            {
                var permissionsInGroup = PermissionsMapper.GetPermissionsInGroup<TPermissionType>(group.Name);

                foreach (var p in permissionsInGroup)
                {
                    if (!PermissionsEvaluator.IsUnlocked(updatedPermission, p) && PermissionsEvaluator.Has(updatedPermission, p, true))
                    {
                        updatedPermission = RemoveFlag(updatedPermission, p);
                    }
                }
            }

            return updatedPermission;
        }

        /// <summary>
        /// Revoke a permission
        /// </summary>
        public static TPermissionType Revoke<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            return Revoke(currentPermission.AsInt(), value).AsPermissions<TPermissionType>();
        }

        private static int AddFlag<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            currentPermission |= PermissionsMapper.AsInt(value);
            return currentPermission;
        }

        private static int AddFlag<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            return AddFlag(PermissionsMapper.AsInt(currentPermission), value);
        }

        private static int RemoveFlag<TPermissionType>(int currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            currentPermission ^= PermissionsMapper.AsInt(value);
            return currentPermission;
        }

        private static int RemoveFlag<TPermissionType>(TPermissionType currentPermission, TPermissionType value)
            where TPermissionType : struct, Enum
        {
            return RemoveFlag(PermissionsMapper.AsInt(currentPermission), value);
        }
    }
}