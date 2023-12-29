using AutoMapper;
using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;
    private readonly IMapper _mapper;

    public LoanService(ILoanRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.LoanType, true, out LoanType loanType))
        {
            throw new ArgumentException("Invalid loan type.");
        }

        requestDto.LoanType = loanType.ToString();
        
        var loanApplication = _mapper.Map<LoanApplication>(requestDto);

        await _repository.CreateLoanApplicationAsync(loanApplication);
    }
}