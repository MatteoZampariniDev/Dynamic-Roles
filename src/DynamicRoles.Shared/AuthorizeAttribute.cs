using DynamicRoles.Core;

namespace DynamicRoles.Shared;

public class AuthorizeAttribute<TPermissionType> : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    where TPermissionType : struct, Enum
{
    protected AuthorizeAttribute() { }

    protected AuthorizeAttribute(string policy) : base(policy) { }

    /// <summary>
    /// '&' operator doesn't work
    /// '|' operator allowed
    /// 'None' and 'All' not allowed
    /// </summary>
    public AuthorizeAttribute(TPermissionType permissions)
    {
        this.Permissions = permissions;
    }

    public TPermissionType Permissions
    {
        get => PolicyNameHelper.GetPermission<TPermissionType>(this.Policy);
        set => this.Policy = PolicyNameHelper.GeneratePolicyNameFor(value);
    }
}
