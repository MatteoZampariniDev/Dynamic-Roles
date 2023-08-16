using DynamicRoles.SharedDemo.Permissions;

namespace DynamicRoles.SharedDemo.Models;

public class PermissionsVm
{
    public IList<Feature1_Permissions> Feature1_Permissions { get; set; } = new List<Feature1_Permissions>();
    public IList<Feature2_Permissions> Feature2_Permissions { get; set; } = new List<Feature2_Permissions>();
}
