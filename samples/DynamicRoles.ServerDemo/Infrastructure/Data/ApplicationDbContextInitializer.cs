using DynamicRoles.ServerDemo.Infrastructure.Identity;
using DynamicRoles.SharedDemo.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DynamicRoles.ServerDemo.Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private const string AdministratorsRole = "Administrators";
    private const string Role1 = "Role 1";
    private const string Role2 = "Role 2";

    private const string adminUserName = "admin@localhost";
    private const string auditorUserName = "auditor@localhost";
    private const string DefaultPassword = "Password123!";

    public ApplicationDbContextInitializer(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task SeedAsync()
    {
        // Create roles
        await _roleManager.CreateAsync(
            new ApplicationRole
            {
                Name = AdministratorsRole,
                NormalizedName = AdministratorsRole.ToUpper(),
                Feature1_Permissions = Feature1_Permissions.All,
                Feature2_Permissions = Feature2_Permissions.All,
            });

        await _roleManager.CreateAsync(
            new ApplicationRole
            {
                Name = Role1,
                NormalizedName = Role1.ToUpper(),
                Feature1_Permissions = Feature1_Permissions.None,
                Feature2_Permissions = Feature2_Permissions.None,
            });

        await _roleManager.CreateAsync(
            new ApplicationRole
            {
                Name = Role2,
                NormalizedName = Role2.ToUpper(),
                Feature1_Permissions = Feature1_Permissions.Permission_3,
                Feature2_Permissions = Feature2_Permissions.Permission_4,
            });

        // Create default admin user
        var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
        await _userManager.CreateAsync(adminUser, DefaultPassword);

        adminUser = await _userManager.FindByNameAsync(adminUserName);
        await _userManager.AddToRoleAsync(adminUser!, AdministratorsRole);

        // Create default auditor user
        var auditorUser = new ApplicationUser { UserName = auditorUserName, Email = auditorUserName };
        await _userManager.CreateAsync(auditorUser, DefaultPassword);

        await _context.SaveChangesAsync();
    }
}
