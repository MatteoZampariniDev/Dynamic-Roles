using DynamicRoles.ClientDemo.Common.Services;
using DynamicRoles.SharedDemo.Models;
using Microsoft.AspNetCore.Components;

namespace DynamicRoles.ClientDemo.Pages;

public partial class Roles
{
    [Inject]
    public IRolesClient RolesClient { get; set; } = default!;
    [Inject]
    public NavigationManager NavManager { get; set; } = null!;

    private RolesVm? RoleVm { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await this.GetRoles();
    }

    private async Task GetRoles()
    {
        this.RoleVm = await this.RolesClient.GetAllAsync();
    }

    private async Task Delete(string id)
    {
        await this.RolesClient.DeleteAsync(id);
        await this.GetRoles();
    }

    private void Edit(string? id = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            this.NavManager.NavigateTo("/role");
        }
        else
        {
            this.NavManager.NavigateTo($"/role/{id}");
        }
    }

    private void EditPermissions()
    {
        this.NavManager.NavigateTo("/permissions");
    }
}
