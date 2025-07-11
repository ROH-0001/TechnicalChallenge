using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Shared.Forms;
using UserManagement.Shared.Models;

namespace UserManagement.Web.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    /// <summary>
    /// Gets all users or filters by active status
    /// </summary>
    /// <param name="isActive">Optional filter to get only active or inactive users</param>
    /// <returns>List of users</returns>
    /// <response code="200">Returns the list of users</response>

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

    /// <summary>
    /// Gets a specific user by ID
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <returns>User details</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(long id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(MapToDto(user));
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user</returns>
    /// <response code="200">Returns the created user</response>
    /// <response code="400">Invalid input data</response>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        //Validate model for server-side validation
        if (!ModelState.IsValid) return BadRequest(ModelState);

        User? user = new User
        {
            Forename = createUserDto.Forename,
            Surname = createUserDto.Surname,
            Email = createUserDto.Email,
            IsActive = createUserDto.IsActive,
            DateOfBirth = createUserDto.DateOfBirth
        };

        User? createdUser = await _userService.CreateAsync(user);
        return Ok(MapToDto(createdUser));
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">The user ID to update</param>
    /// <param name="updateUserDto">Updated user data</param>
    /// <returns>Updated user</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="404">User not found</response>
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

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">The user ID to delete</param>
    /// <returns>No content on success</returns>
    /// <response code="204">User successfully deleted</response>
    /// <response code="404">User not found</response>
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
