using UserManagement.Shared.Forms;
using UserManagement.Shared.Models;

namespace UserManagement.Web.Client.Services.Interfaces;

public interface IUserApiService
{
    Task<UserListDto?> GetUsersAsync(bool? isActive = null);
    Task<UserDto?> GetUserByIdAsync(long id);
    Task<UserDto?> CreateAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateAsync(long id,UpdateUserDto updateUserDto);
    Task<bool> DeleteAsync(long id);
}
