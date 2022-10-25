using AutoMapper;
using Moq;
using OSD_HR_Management_Backend.Logics.Implementations;
using OSD_HR_Management_Backend.Mappers;
using OSD_HR_Management_Backend.Repositories.Abstractions;
using OSD_HR_Management_Backend.Repositories.Models;
using OSD_HR_Management_Backend.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OSD_HR_Management_Backend.Tests.Logic;

public class UserLogicTest
{
    #region Private Fields
    private readonly Mock<IUserRepository> _userRepository;

    private const string EXISTING_USER_ID = "a961157c-8880-2022-89be-a83dc298d302";
    private const string NON_EXISTING_USER_ID = "non-existing-id";    
    #endregion
    public UserLogicTest()
    {
        _userRepository = new Mock<IUserRepository>();
    }
    #region Setup
    private UserLogic GetLogic()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UserMapperProfile());
        });
        var mapper = config.CreateMapper();
        return new UserLogic(_userRepository.Object, mapper);
    }

    private void SetUpService()
    {
        _userRepository
                    .Setup(x => x.SaveUser(It.IsAny<UserModel>()))
                    .Verifiable();

        _userRepository
                    .Setup(x => x.GetAllUsers())
                    .Returns(Task.FromResult(new List<UserModel>
                    {
                        new UserModel
                        {
                            UserId = "1",
                            Username = "user1",
                            Password = "123456",
                            Avatar = "avatar",
                            Role = "Employee",
                            IsActive = true,
                            JobTitle = "Software",
                            FullName = "hahaha"
                        },
                        new UserModel
                        {
                            UserId = "2",
                            Username = "admin",
                            Password = "123456",
                            Avatar = "avatar",
                            Role = "Admin",
                            IsActive = true,
                            JobTitle = "Admin",
                            PhoneNumber = "2596325596"
                        },
                        new UserModel
                        {
                            UserId = "3",
                            Username = "user2",
                            Password = "123456",
                            Avatar = "avatar",
                            Role = "Employee",
                            IsActive = true,
                            JobTitle = "Software 1",
                            Skype = "live:6952180"
                        }
                    }));

        _userRepository
                    .Setup(x => x.GetUserById(EXISTING_USER_ID))
                    .Returns(Task.FromResult(new UserModel
                    {
                        UserId = "a961157c-8880-2022-89be-a83dc298d302",
                        Username = "user1",
                        Password = "123456",
                        Avatar = "avatar",
                        Role = "Employee",
                        IsActive = true,
                        JobTitle = "Software",
                        FullName = "hahaha"
                    }));
    }
    #endregion

    #region Tests
    [Fact]
    public async Task TestSaveUser_WithValidRegisterRequestModel_SaveNewUserSuccessfully()
    {
        // Arrange
        SetUpService();

        var logic = GetLogic();

        // Act
        await logic.SaveUser(new RegisterRequestModel {
            Username = "username",
            Password = "password"
        }, "avatar");

        // Assert
        _userRepository.Verify(x => x.SaveUser(It.IsAny<UserModel>()), Times.Once);
    }

    [Fact]
    public async Task TestGetUsersPortal_ReturnListWithItems()
    {
        // Arrange
        SetUpService();

        var logic = GetLogic();

        // Act
        var result = await logic.GetUsersPortal();

        // Assert
        Assert.True(result?.Count > 0);
    }

    [Fact]
    public async Task TestGetUserInfo_WithExistingId_ReturnCorrectUser()
    {
        // Arrange
        SetUpService();

        var logic = GetLogic();

        // Act
        var userInfo = await logic.GetUserById(EXISTING_USER_ID);

        // Assert
        Assert.Equal(EXISTING_USER_ID, userInfo.UserId);
    }

    [Fact]
    public async Task TestGetUserInfo_WithNonExistingId_ThrowException()
    {
        // Arrange
        SetUpService();

        var logic = GetLogic();

        var EXPECTED_ERROR_MESSAGE = $"User with Id {NON_EXISTING_USER_ID} does not exist";
        var errorMessage = string.Empty;

        // Act
        try
        {
            await logic.GetUserById(NON_EXISTING_USER_ID);
        }
        catch(Exception ex)
        {
            errorMessage = ex.Message;
        }

        // Assert
        Assert.Equal(EXPECTED_ERROR_MESSAGE, errorMessage);
    }
    #endregion
}
