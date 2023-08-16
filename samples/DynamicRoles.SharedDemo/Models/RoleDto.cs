using DynamicRoles.SharedDemo.Permissions;

namespace DynamicRoles.SharedDemo.Models;
public class RoleDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public Feature1_Permissions Feature1_Permissions { get; set; } = Feature1_Permissions.None;
    public Feature2_Permissions Feature2_Permissions { get; set; } = Feature2_Permissions.None;
}
