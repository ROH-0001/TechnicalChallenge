using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Shared.Forms;
using UserManagement.Shared.Models;

namespace UserManagement.Web.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    //TODO: Documentation for swagger
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


    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(MapToDto(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        //Validate model for server-side validation
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new User
        {
            Forename = createUserDto.Forename,
            Surname = createUserDto.Surname,
            Email = createUserDto.Email,
            IsActive = createUserDto.IsActive,
            DateOfBirth = createUserDto.DateOfBirth
        };

        var createdUser = await _userService.CreateAsync(user);
        return Ok(MapToDto(createdUser));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(long id, UpdateUserDto updateUserDto)
    {
        //Validate model for server-side validation
        if (!ModelState.IsValid) return BadRequest(ModelState);

        User user = new User
        {
            Id = id,
            Forename = updateUserDto.Forename,
            Surname = updateUserDto.Surname,
            Email = updateUserDto.Email,
            IsActive = updateUserDto.IsActive,
            DateOfBirth = updateUserDto.DateOfBirth
        };

        User? updatedUser = await _userService.UpdateAsync(user);
        if (updatedUser == null) return NotFound();

        return Ok(MapToDto(updatedUser));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(long id)
    {
        bool success = await _userService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        IsActive = user.IsActive,
        DateOfBirth = user.DateOfBirth
    };

}
