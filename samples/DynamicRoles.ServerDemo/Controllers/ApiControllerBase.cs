using Microsoft.AspNetCore.Mvc;

namespace DynamicRoles.ServerDemo.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{

}
