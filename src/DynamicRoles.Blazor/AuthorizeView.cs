using DynamicRoles.Core;
using Microsoft.AspNetCore.Components;

namespace DynamicRoles.Blazor;

public class AuthorizeView<PermissionsType> : Microsoft.AspNetCore.Components.Authorization.AuthorizeView
    where PermissionsType : struct, Enum
{
    /// <summary>
    /// '&' operator doesn't work
    /// '|' operator allowed
    /// 'None' and 'All' not allowed
    /// </summary>
    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public PermissionsType Permissions
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => PolicyNameHelper.GetPermission<PermissionsType>(this.Policy);
        set => this.Policy = PolicyNameHelper.GeneratePolicyNameFor(value);
    }
}
