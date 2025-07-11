using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAllAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public async Task FilterByActive_WhenActiveTrue_ShouldReturnActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsersWithActiveStatus();

        // Act
        var result = await service.FilterByActiveAsync(true);

        // Assert
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task FilterByActive_WhenActiveFalse_ShouldReturnInactiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsersWithActiveStatus();

        // Act
        var result = await service.FilterByActiveAsync(false);

        // Assert
        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();

        // Act
        var result = await service.GetUserByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();

        // Act
        var result = await service.GetUserByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenValidUser_ShouldCallDataContextAndReturnUser()
    {
        // Arrange
        var service = CreateService();
        var user = CreateUser();

        // Act
        var result = await service.CreateAsync(user);

        // Assert
        _dataContext.Verify(x => x.CreateAsync(user), Times.Once);
        result.Should().BeSameAs(user);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserExists_ShouldUpdateAndReturnUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var updateUser = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "User",
            Email = "updated@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var result = await service.UpdateAsync(updateUser);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("updated@example.com");
        _dataContext.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var updateUser = new User { Id = 999 };

        // Act
        var result = await service.UpdateAsync(updateUser);

        // Assert
        result.Should().BeNull();
        _dataContext.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _dataContext.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();

        // Act
        var result = await service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _dataContext.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime? dateOfBirth = null)
    {
        var users = new[]
        {
            new User
            {
                Id = 1,
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth ?? new DateTime(1990, 1, 1)
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAllAsync<User>())
            .ReturnsAsync(users);

        return users;
    }

    private IQueryable<User> SetupUsersWithActiveStatus()
    {
        var users = new[]
        {
            new User
            {
                Id = 1,
                Forename = "Active",
                Surname = "User",
                Email = "active@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 1, 1)
            },
            new User
            {
                Id = 2,
                Forename = "Inactive",
                Surname = "User",
                Email = "inactive@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(1985, 5, 15)
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAllAsync<User>())
            .ReturnsAsync(users);

        return users;
    }

    private User CreateUser() => new()
    {
        Forename = "New",
        Surname = "User",
        Email = "new@example.com",
        IsActive = true,
        DateOfBirth = new DateTime(1992, 3, 10)
    };

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
