using DynamicRoles.ClientDemo.Common.Services;
using DynamicRoles.ClientDemo.Pages.Shared;
using DynamicRoles.SharedDemo.Models;
using Microsoft.AspNetCore.Components;

namespace DynamicRoles.ClientDemo.Pages;

public partial class Permissions : CommonComponent
{
    [Inject]
    public IRolesClient RolesClient { get; set; } = default!;

    private RolesVm? RolesVm { get; set; }
    private PermissionsVm? PermissionsVm { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await this.LoadDataAsync();
    }

    protected async Task LoadDataAsync()
    {
        this.RolesVm = await this.RolesClient.GetAllAsync();
        this.PermissionsVm = await this.RolesClient.GetPermissionsAsync();
    }
}