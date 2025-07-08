using System;
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
    public Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _dataAccess.GetAllAsync<User>();
}
