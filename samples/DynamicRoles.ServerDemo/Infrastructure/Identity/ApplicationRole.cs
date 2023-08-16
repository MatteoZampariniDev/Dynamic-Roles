using DynamicRoles.SharedDemo.Permissions;
using Microsoft.AspNetCore.Identity;

namespace DynamicRoles.ServerDemo.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public Feature1_Permissions Feature1_Permissions { get; set; }
    public Feature2_Permissions Feature2_Permissions { get; set; }
}
