using AutoMapper;
using BankProject.API.DTOs.User;
using BankProject.Business.DTOs.Account;
using BankProject.Business.DTOs.User;
using BankProject.Data.Entities;

namespace BankProject.Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        #region Account Mappings

        CreateMap<Account, CreateAccountRequestDto>().ReverseMap();

        #endregion
    }
}