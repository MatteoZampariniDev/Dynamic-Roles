using DynamicRoles.Shared;
using DynamicRoles.SharedDemo.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace DynamicRoles.ServerDemo.Controllers;

public class Feature1Controller : ApiControllerBase
{
    [Authorize<Feature1_Permissions>(Feature1_Permissions.Permission_1)]
    [HttpGet]
    public string RequiresPermission1() => "Feature1_Endpoint 1";

    [Authorize<Feature1_Permissions>(Feature1_Permissions.Permission_2)]
    [HttpGet]
    public string RequiresPermission2() => "Feature1_Endpoint 2";

    [Authorize<Feature1_Permissions>(Feature1_Permissions.Permission_2 | Feature1_Permissions.Permission_5)]
    [HttpGet]
    public string RequiresPermission2_Or_Permission5() => "Feature1_Endpoint 3";
}
