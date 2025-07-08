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
   public async Task<ActionResult<UserListDto>> GetUsersAsync([FromQuery] bool? isActive = null)
    {
        IEnumerable<Models.User> users = isActive.HasValue 
            ? await _userService.FilterByActiveAsync(isActive.Value)
            : await _userService.GetAllAsync();

        IEnumerable<UserDto> usersDtos = users.Select(p => new UserDto
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        UserListDto result = new UserListDto
        {
            Items = usersDtos.ToList()
        };

        return Ok(result);
    }
}
