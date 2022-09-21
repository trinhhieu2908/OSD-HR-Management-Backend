using OSD_HR_Management_Backend.Mappers;

namespace OSD_HR_Management_Backend.ExtensionMethods;

public static class AutoMapperConfigurations
{
    public static void ConfigureAutoMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMapperProfile).Assembly);
    }
}
