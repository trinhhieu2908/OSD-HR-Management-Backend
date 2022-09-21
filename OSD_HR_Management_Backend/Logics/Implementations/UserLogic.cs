using AutoMapper;
using OSD_HR_Management_Backend.Logics.Abstractions;
using OSD_HR_Management_Backend.Logics.Helpers;
using OSD_HR_Management_Backend.Repositories.Abstractions;
using OSD_HR_Management_Backend.Repositories.Models;

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
        user.CreateAt = DateTime.Now;
        user.Password = EncodingHelper.EncodePasswordToBase64(requestModel.Password);
        user.Avatar = avatarPath;

        var userId = await _userRepository.SaveUser(user);

        return userId;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }
}
