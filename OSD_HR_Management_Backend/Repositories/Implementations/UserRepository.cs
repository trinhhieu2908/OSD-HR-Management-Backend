using Amazon.DynamoDBv2.DataModel;
using OSD_HR_Management_Backend.Repositories.Abstractions;
using OSD_HR_Management_Backend.Repositories.Models;

namespace OSD_HR_Management_Backend.Repositories.Implementations;

public class UserRepository : HashKeyOnlyRepositoryBase<UserModel, string>, IUserRepository
{
    public UserRepository(IDynamoDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<string> SaveUser(UserModel user)
    {        
        await SaveAsync(user);

        return user.UserId;
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        return await GetList();
    }
}
