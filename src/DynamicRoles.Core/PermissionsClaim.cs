using Newtonsoft.Json;
using System.Collections.Generic;

namespace DynamicRoles.Core
{
    public class PermissionsClaim
    {
        public const string CLAIM_NAME = "permissions";

        protected Dictionary<string, int> UserPermissions { get; set; } = new Dictionary<string, int>();

        protected PermissionsClaim() { }
        public PermissionsClaim(Dictionary<string, int> userPermissions)
        {
            this.UserPermissions = userPermissions;
        }

        public bool Validate(string permissionUniqueName, int requiredPermission)
        {
            return this.UserPermissions.TryGetValue(permissionUniqueName, out int userPermissionValue)
                && (userPermissionValue & requiredPermission) != 0;
        }

        public string ToClaimValue()
        {
            return JsonConvert.SerializeObject(this.UserPermissions, Formatting.None);
        }

        public static PermissionsClaim Parse(string claimValue)
        {
            try
            {
                return new PermissionsClaim()
                {
                    UserPermissions = JsonConvert.DeserializeObject<Dictionary<string, int>>(claimValue)
                        ?? new Dictionary<string, int>()
                };
            }
            catch
            {
                return new PermissionsClaim();
            }
        }
    }
}