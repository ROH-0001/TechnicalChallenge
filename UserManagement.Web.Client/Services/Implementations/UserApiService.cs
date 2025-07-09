using System.Net.Http.Json;
using UserManagement.Shared.Forms;
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
            UserListDto? userList = await _httpClient.GetFromJsonAsync<UserListDto>(url);
            return userList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return null;
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(long id)
    {
        try
        {
            string url = $"api/users/{id}";
            UserDto? user = await _httpClient.GetFromJsonAsync<UserDto>(url);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return null;
        }
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto createUserDto)
    {
        try
        {
            string url = "api/users";
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(url, createUserDto);
            UserDto? user = await httpResponse.Content.ReadFromJsonAsync<UserDto>();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return null;
        }
    }

    public async Task<UserDto?> UpdateAsync(long id, UpdateUserDto updateUserDto)
    {
        try
        {
            string url = $"api/users/{id}";
            HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(url, updateUserDto);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            UserDto? user = await httpResponse.Content.ReadFromJsonAsync<UserDto>();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            string url = $"api/users/{id}";
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(url);
            return httpResponse.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return false;
        }
    }


}
