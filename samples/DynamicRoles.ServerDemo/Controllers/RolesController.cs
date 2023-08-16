using DynamicRoles.Core;
using DynamicRoles.ServerDemo.Infrastructure.Data;
using DynamicRoles.ServerDemo.Infrastructure.Identity;
using DynamicRoles.SharedDemo.Models;
using DynamicRoles.SharedDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicRoles.ServerDemo.Controllers;

public class RolesController : ApiControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public RolesController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
    {
        this._roleManager = roleManager;
        this._context = context;
    }

    #region Queries
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<RoleDto?>> Get(string id)
    {
        return ToDto(await this._roleManager.FindByIdAsync(id));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<RolesVm>> GetAll()
    {
        var roles = await _context.Roles.AsNoTracking().ToListAsync();

        var result = new RolesVm();
        result.Roles.AddRange(roles.Where(r => r != null).Select(r => ToDto(r)!));

        return result;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<PermissionsVm> GetPermissions()
    {
        return new PermissionsVm
        {
            Feature1_Permissions = PermissionsMapper.GetAll<Feature1_Permissions>(),
            Feature2_Permissions = PermissionsMapper.GetAll<Feature2_Permissions>(),
        };
    }
    #endregion

    #region Commands
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create(RoleDto entity)
    {
        await _roleManager.CreateAsync(ToApplicationRole(entity));
        return this.NoContent();
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(string id, RoleDto entity)
    {
        if (id != entity.Id)
            return this.BadRequest();

        var dbEntity = await this._roleManager.FindByIdAsync(id);
        if (dbEntity != null)
        {
            await _roleManager.UpdateAsync(ToApplicationRole(entity, dbEntity));
        }
        
        return this.NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(string id)
    {
        var entity = await this._roleManager.FindByIdAsync(id);

        if (entity != null)
        {
            await _roleManager.DeleteAsync(entity);
        }
        
        return this.NoContent();
    }
    #endregion

    private static RoleDto? ToDto(ApplicationRole? applicationRole)
    {
        return applicationRole == null? null : new RoleDto
        {
            Id = applicationRole.Id,
            Name = applicationRole.Name,
            Feature1_Permissions = applicationRole.Feature1_Permissions,
            Feature2_Permissions = applicationRole.Feature2_Permissions,
        };
    }

    private static ApplicationRole ToApplicationRole(RoleDto roleDto, ApplicationRole? existingRole = null)
    {
        if (existingRole != null)
        {
            existingRole.Name = roleDto.Name;
            existingRole.Feature1_Permissions = roleDto.Feature1_Permissions;
            existingRole.Feature2_Permissions = roleDto.Feature2_Permissions;
            return existingRole;
        }
        else
        {
            return new ApplicationRole 
            { 
                Name = roleDto.Name,
                Feature1_Permissions = roleDto.Feature1_Permissions,
                Feature2_Permissions = roleDto.Feature2_Permissions,
            };
        }
    }
}
