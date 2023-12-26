using AutoMapper;
using BankProject.API.DTOs.User;
using BankProject.Data.Entities;

namespace BankProject.Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        #region User Mappings

        CreateMap<User, CreateUserRequestDto>().ReverseMap();

        CreateMap<User, GetUserRequestDto>().ReverseMap();

        CreateMap<User, UpdateUserRequestDto>().ReverseMap();
        
        #endregion
        
        
    }
}