using AutoMapper;
using BankProject.API.DTOs.User;
using BankProject.Business.DTOs.Account;
using BankProject.Business.DTOs.Role;
using BankProject.Business.DTOs.User;
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

        #region Account Mappings

        CreateMap<Account, CreateAccountRequestDto>().ReverseMap();

        #endregion

        #region Role Mappings

        CreateMap<Role, RoleDto>().ReverseMap();

        #endregion
    }
}