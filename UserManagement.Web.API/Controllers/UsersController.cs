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
    public async Task<ActionResult<UserListDto>> GetUsersAsync()
    {
        var users = (await _userService.GetAllAsync()).Select(p => new UserDto
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
