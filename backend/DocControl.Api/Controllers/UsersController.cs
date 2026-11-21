using DocControl.Api.Models;
using DocControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserListItems>> GetUsers()
    {
        return Ok(_userService.GetUsers());
    }

    [HttpPut("{employeeId}")]
    public IActionResult Update(string employeeId, [FromBody] UpdateUserRequest request)
    {
        return ToResponse(_userService.Update(employeeId, request));
    }

    [HttpPatch("{employeeId}/status")]
    public IActionResult SetStatus(string employeeId, [FromBody] SetStatusRequest request)
    {
        return ToResponse(_userService.SetStatus(employeeId, request.Active));
    }

    [HttpDelete("{employeeId}")]
    public IActionResult Delete(string employeeId)
    {
        return ToResponse(_userService.Delete(employeeId));
    }

    private IActionResult ToResponse(UserResult result)
    {
        return result.Status switch
        {
            UserOpStatus.Ok => NoContent(),
            UserOpStatus.NotFound => NotFound(new { message = result.Message }),
            UserOpStatus.Conflict => Conflict(new { message = result.Message }),
            _=> BadRequest( new {message = result.Message})
        };
    }
}
