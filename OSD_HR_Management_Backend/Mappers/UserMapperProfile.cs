using AutoMapper;
using OSD_HR_Management_Backend.Repositories.Models;
using OSD_HR_Management_Backend.ResponseModels;

namespace OSD_HR_Management_Backend.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterRequestModel, UserModel>();
        CreateMap<UserModel, GetUserResponseModel>();
    }
}
