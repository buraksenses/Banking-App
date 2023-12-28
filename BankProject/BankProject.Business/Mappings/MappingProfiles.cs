using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.DTOs.User;
using BankProject.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        #region Account Mappings

        CreateMap<Account, CreateAccountRequestDto>().ReverseMap();

        #endregion

        #region User Mappings

        CreateMap<User, CreateUserRequestDto>().ReverseMap();

        #endregion
    }
}