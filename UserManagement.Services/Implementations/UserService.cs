using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
    {
        IQueryable<User> AllUsers = await _dataAccess.GetAllAsync<User>();
        return AllUsers.Where(x => x.IsActive == isActive);
    }
    public async Task<IEnumerable<User>> GetAllAsync() => await _dataAccess.GetAllAsync<User>();

    public async Task<User?> GetUserByIdAsync(long id)
    {
        var users = await _dataAccess.GetAllAsync<User>();
        return users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var existingUser = await GetUserByIdAsync(user.Id);
        if (existingUser == null) return null;

        //remap values onto the already tracked entity to get rid of duplicate key error trying to pass `user`
        existingUser.Forename = user.Forename;
        existingUser.Surname = user.Surname;
        existingUser.Email = user.Email;
        existingUser.IsActive = user.IsActive;
        existingUser.DateOfBirth = user.DateOfBirth;

        await _dataAccess.UpdateAsync(existingUser);
        return existingUser;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await GetUserByIdAsync(id);
        if (user == null) return false;

        await _dataAccess.DeleteAsync(user);
        return true;
    }
}
