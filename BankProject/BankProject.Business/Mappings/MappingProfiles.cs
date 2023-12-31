using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.DTOs.Loan;
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

        #region User Mappings

        CreateMap<User, CreateUserRequestDto>().ReverseMap();

        #endregion

        #region Loan Mappings

        CreateMap<LoanApplication, CreateLoanApplicationRequestDto>().ReverseMap();

        CreateMap<LoanApplication, GetLoanApplicationRequestDto>().ReverseMap();

        CreateMap<Loan, LoanApplication>()
            .ForMember(dest => dest.LoanTerm, opt => opt.MapFrom(src => src.LoanTerm))
            .ForMember(dest => dest.LoanType, opt => opt.MapFrom(src => src.LoanType))
            .ForMember(dest => dest.LoanAmount, opt => opt.MapFrom(src => src.LoanAmount))
            .ForMember(dest => dest.LoanAmount, opt => opt.MapFrom(src => src.RemainingDebt))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();

        CreateMap<Loan, CreateLoanRequestDto>().ReverseMap();

        #endregion
    }
}