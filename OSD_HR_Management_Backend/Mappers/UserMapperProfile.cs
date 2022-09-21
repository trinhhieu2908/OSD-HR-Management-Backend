using AutoMapper;
using OSD_HR_Management_Backend.Repositories.Models;

namespace OSD_HR_Management_Backend.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterRequestModel, UserModel>();
    }
}
