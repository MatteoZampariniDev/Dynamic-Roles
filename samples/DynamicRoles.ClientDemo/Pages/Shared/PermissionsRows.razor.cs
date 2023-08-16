using DynamicRoles.ClientDemo.Common.Services;
using DynamicRoles.Core;
using DynamicRoles.SharedDemo.Models;
using Microsoft.AspNetCore.Components;

namespace DynamicRoles.ClientDemo.Pages.Shared;
public partial class PermissionsRows<TPermissionType> : CommonComponent
    where TPermissionType : struct, Enum
{
    [Inject]
    public IRolesClient RolesClient { get; set; } = default!;

    [Parameter]
    public IList<TPermissionType>? PermissionsData { get; set; }

    [Parameter]
    public RolesVm? RolesVm { get; set; }

    private async Task SetAsync(RoleDto role, TPermissionType value, bool granted)
    {
        PermissionsUpdater.GrantOrRevoke(role, value, granted);
        await this.UpdateAsync(role);
    }

    private async Task SetAllAsync(RoleDto role, bool granted)
    {
        PermissionsUpdater.GrantOrRevokeAll<RoleDto, TPermissionType>(role, granted);
        await this.UpdateAsync(role);
    }

    private async Task UpdateAsync(RoleDto? role = null)
    {
        if (role != null)
        {
            await this.RolesClient.UpdateAsync(role.Id, role);
        }

        this.RolesVm = await this.RolesClient.GetAllAsync();
    }

    #region UI Helpers
    private string PermissionsTypeName()
    {
        var name = PermissionsMapper.GetDetailsAttribute<TPermissionType>().Name;
        return !string.IsNullOrWhiteSpace(name) ? name : typeof(TPermissionType).Name;
    }
    private string PermissionsTypeDescription()
    {
        return PermissionsMapper.GetDetailsAttribute<TPermissionType>().Description;
    }

    private string PermissionsTooltip(RoleDto role, TPermissionType permissions)
    {
        return !PermissionsEvaluator.IsUnlockedPermission(role, permissions)
            ? $"Permission Level Locked"
            : PermissionsEvaluator.HasPermission(role, permissions) ? "Revoke" : "Grant";
    }

    private string PermissionNameAndGroup(TPermissionType value)
    {
        string result = string.Empty;

        var group = PermissionsMapper.GetGroupAttribute(value);
        if (!string.IsNullOrWhiteSpace(group.Name))
        {
            result = $"[{group.Name} - Level: {group.Level}] ";
        }

        var descrAttr = PermissionsMapper.GetDetailsAttribute(value);
        if (!string.IsNullOrWhiteSpace(descrAttr.Name))
        {
            result += descrAttr.Name;
        }
        else
        {
            result += value.ToString();
        }

        return result;
    }

    private string PermissionDescription(TPermissionType value)
    {
        return PermissionsMapper.GetDetailsAttribute(value).Description;
    }
    #endregion
}