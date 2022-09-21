using OSD_HR_Management_Backend.Logics.Abstractions;
using OSD_HR_Management_Backend.Logics.Implementations;
using OSD_HR_Management_Backend.Repositories.Abstractions;
using OSD_HR_Management_Backend.Repositories.Implementations;

namespace OSD_HR_Management_Backend.ExtensionMethods;

public static class RepositoriesConfigurations
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserLogic, UserLogic>();        
    }
}
