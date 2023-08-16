using System;

namespace DynamicRoles.Core
{
    public static class PolicyNameHelper
    {
        // PERMISSION_UNIQUE_NAME#PERMISSION_VALUE
        public const string SEPARATOR = "#";

        public static bool IsValidPolicyName(string? policyName)
        {
            return policyName != null && policyName.Contains(SEPARATOR);
        }

        public static string GeneratePolicyNameFor<TPermissionType>(TPermissionType permissions)
            where TPermissionType : struct, Enum
        {
            int permissionValue = PermissionsMapper.AsInt(permissions);

            if (permissionValue == 0 || permissionValue == -1)
            {
                throw new PermissionsException($"[{nameof(PolicyNameHelper)}] Required permission cannot be None (0) or All (-1) (provided: {permissions}).");
            }

            var permissionsAttr = PermissionsMapper.GetPermissionsAttribute<TPermissionType>();

            if (permissionsAttr == null)
            {
                throw new PermissionsException($"[{nameof(PolicyNameHelper)}] {nameof(PermissionsAttribute)} not found for {typeof(TPermissionType).FullName}");
            }

            return permissionValue != 0
                ? $"{permissionsAttr.UniqueName}{SEPARATOR}{permissionValue}"
                : string.Empty;
        }

        public static TPermissions GetPermission<TPermissions>(string? policyName)
            where TPermissions : struct, Enum
        {
            return PermissionsMapper.AsPermissions<TPermissions>(
                !string.IsNullOrEmpty(policyName) ? GetPermissionValueFrom(policyName) : 0);
        }

        public static string GetPermissionsUniqueName(string policyName)
        {
            int separatorIndex = policyName.IndexOf(SEPARATOR);
            return policyName.Remove(separatorIndex);
        }

        public static int GetPermissionValueFrom(string policyName)
        {
            return int.Parse(policyName[(GetPermissionsUniqueName(policyName).Length + 1)..]!);
        }
    }
}