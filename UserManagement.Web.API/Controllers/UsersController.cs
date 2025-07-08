using UserManagement.Services.Domain.Interfaces;
using UserManagement.Shared.Models;

namespace UserManagement.Web.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ActionResult<UserListDto> GetUsers()
    {
        var users = _userService.GetAll().Select(p => new UserDto
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var result = new UserListDto
        {
            Items = users.ToList()
        };

        return Ok(result); 
    }
}
