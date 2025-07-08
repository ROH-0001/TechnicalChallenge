using UserManagement.Shared.Models;

namespace UserManagement.Web.Client.Services.Interfaces;

public interface IUserApiService
{
    Task<UserListDto?> GetUsersAsync(bool? isActive = null);
}
