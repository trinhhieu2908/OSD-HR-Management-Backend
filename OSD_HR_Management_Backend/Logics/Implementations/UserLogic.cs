using AutoMapper;
using OSD_HR_Management_Backend.Logics.Abstractions;
using OSD_HR_Management_Backend.Logics.Helpers;
using OSD_HR_Management_Backend.Repositories.Abstractions;
using OSD_HR_Management_Backend.Repositories.Models;
using OSD_HR_Management_Backend.ResponseModels;

namespace OSD_HR_Management_Backend.Logics.Implementations;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserLogic(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<string> SaveUser(RegisterRequestModel requestModel, string avatarPath)
    {
        var user = _mapper.Map<RegisterRequestModel, UserModel>(requestModel);

        user.UserId = Guid.NewGuid().ToString();        
        user.Password = EncodingHelper.EncodePasswordToBase64(requestModel.Password);
        user.Avatar = avatarPath;
        user.IsActive = true;
        user.CreateAt = DateTime.Now;
        user.UpdateAt = DateTime.Now;

        var userId = await _userRepository.SaveUser(user);

        return userId;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<List<UserResponseModel>> GetUsersPortal()
    {
        var usersResponse = new List<UserResponseModel>();

        var users = await _userRepository.GetAllUsers();

        foreach (var user in users)
        {
            var userResponse = _mapper.Map<UserModel, UserResponseModel>(user);
            usersResponse.Add(userResponse);
        }

        return usersResponse;
    }

    public async Task<UserResponseModel> GetUserById(string userId)
    {
        var user = await _userRepository.GetUserById(userId);

        if(user == null)
        {
            throw new Exception($"User with Id {userId} not exist");
        }

        var userResponse = _mapper.Map<UserModel, UserResponseModel>(user);

        return userResponse;
    }
}
