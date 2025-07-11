using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Shared.Models;
using UserManagement.Web.API.Controllers;
using UserManagement.Shared.Forms;

namespace UserManagement.Web.Api.Tests;
public class UsersControllerTests
{
    [Fact]
    public async Task GetUsers_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.GetUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Result
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListDto>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetUsers_WhenFilteredByActive_ShouldCallFilterByActiveAsync()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupActiveUsers();

        // Act
        var result = await controller.GetUsersAsync(isActive: true);

        // Assert
        _userService.Verify(s => s.FilterByActiveAsync(true), Times.Once);
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListDto>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetUser_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var controller = CreateController();
        var user = SetupSingleUser();

        // Act
        var result = await controller.GetUserAsync(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserDto>()
            .Which.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUser_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(s => s.GetUserByIdAsync(999)).ReturnsAsync(null as User);

        // Act
        var result = await controller.GetUserAsync(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateUser_WhenValidUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var controller = CreateController();
        var createUserDto = CreateUserDto();
        SetupCreateUserResponse(createUserDto);

        // Act
        var result = await controller.CreateUserAsync(createUserDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserDto>()
            .Which.Should().BeEquivalentTo(createUserDto); //This excludes the ID check automatically between model and form
    }

    [Fact]
    public async Task UpdateUser_WhenUserExists_ShouldReturnUpdatedUser()
    {
        // Arrange
        var controller = CreateController();
        var updateUserDto = UpdateUserDto();
        SetupUpdateUserResponse(updateUserDto);

        // Act
        var result = await controller.UpdateUserAsync(1, updateUserDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserDto>()
            .Which.Should().BeEquivalentTo(updateUserDto); //This excludes the ID check automatically between model and form
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        var updateUserDto = UpdateUserDto();
        _userService.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(null as User);

        // Act
        var result = await controller.UpdateUserAsync(999, updateUserDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ShouldReturnNoContent()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await controller.DeleteUserAsync(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var controller = CreateController();
        _userService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await controller.DeleteUserAsync(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetUsers_WhenFilteredByActiveStatus_ShouldCallCorrectServiceMethod(bool isActive)
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: isActive);
        _userService.Setup(s => s.FilterByActiveAsync(isActive)).ReturnsAsync(users);

        // Act
        var result = await controller.GetUsersAsync(isActive);

        // Assert
        _userService.Verify(s => s.FilterByActiveAsync(isActive), Times.Once);
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListDto>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime? dateOfBirth = null)
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
        };

        _userService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users);

        return users;
    }

    private User[] SetupActiveUsers()
    {
        var users = new[]
        {
            new User
            {
                Id = 1,
                Forename = "Johnny",
                Surname = "User",
                Email = "juser@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 1, 1)
            }
        };

        _userService.Setup(s => s.FilterByActiveAsync(true)).ReturnsAsync(users);

        return users;
    }

    private User SetupSingleUser()
    {
        var user = new User
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _userService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

        return user;
    }

    private CreateUserDto CreateUserDto() => new()
    {
        Forename = "Johnny",
        Surname = "Silverhand",
        Email = "jsilverhand@example.com",
        IsActive = true,
        DateOfBirth = new DateTime(1988, 11, 16)
    };

    private UpdateUserDto UpdateUserDto() => new()
    {
        Forename = "Johnny",
        Surname = "Updated",
        Email = "jupdated@example.com",
        IsActive = false,
        DateOfBirth = new DateTime(1990, 1, 1)
    };

    private void SetupCreateUserResponse(CreateUserDto createUserDto)
    {
        var user = new User
        {
            Id = 1,
            Forename = createUserDto.Forename,
            Surname = createUserDto.Surname,
            Email = createUserDto.Email,
            IsActive = createUserDto.IsActive,
            DateOfBirth = createUserDto.DateOfBirth
        };

        _userService.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);

    }

    private void SetupUpdateUserResponse(UpdateUserDto updateUserDto)
    {
        var user = new User
        {
            Id = 1,
            Forename = updateUserDto.Forename,
            Surname = updateUserDto.Surname,
            Email = updateUserDto.Email,
            IsActive = updateUserDto.IsActive,
            DateOfBirth = updateUserDto.DateOfBirth
        };

        _userService.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
