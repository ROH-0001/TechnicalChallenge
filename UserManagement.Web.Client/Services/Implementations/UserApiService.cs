using System.Net.Http.Json;
using UserManagement.Shared.Models;
using UserManagement.Web.Client.Services.Interfaces;

namespace UserManagement.Web.Client.Services.Implementations;

public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserListDto?> GetUsersAsync(bool? isActive = null)
    {
        try
        {
            string url = isActive.HasValue ? $"api/users?isActive={isActive.Value}" : "api/users";
            UserListDto? response = await _httpClient.GetFromJsonAsync<UserListDto>(url);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return null;
        }
    }
}
