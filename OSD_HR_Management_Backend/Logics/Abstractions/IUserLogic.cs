using OSD_HR_Management_Backend.Repositories.Models;

namespace OSD_HR_Management_Backend.Logics.Abstractions;

public interface IUserLogic
{
    Task<string> SaveUser(RegisterRequestModel requestModel, string avatarPath);
    Task<IEnumerable<UserModel>> GetAllUsers();
}
