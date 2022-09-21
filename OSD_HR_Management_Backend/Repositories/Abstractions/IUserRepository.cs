using OSD_HR_Management_Backend.Repositories.Models;

namespace OSD_HR_Management_Backend.Repositories.Abstractions;

public interface IUserRepository
{
    Task<string> SaveUser(UserModel user);
    Task<List<UserModel>> GetAllUsers();
}
