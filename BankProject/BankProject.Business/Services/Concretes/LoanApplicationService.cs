using AutoMapper;
using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;

    public LoanApplicationService(ILoanApplicationRepository applicationRepository,IMapper mapper)
    {
        _applicationRepository = applicationRepository;
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

        await _applicationRepository.CreateLoanApplicationAsync(loanApplication);
    }

    public async Task<GetLoanApplicationRequestDto> GetLoanApplicationByIdAsync(Guid id)
    {
        var application = await GetLoanApplicationOrThrow(id);

        var applicationDto = _mapper.Map<GetLoanApplicationRequestDto>(application);

        return applicationDto;
    }
    
    private async Task<LoanApplication> GetLoanApplicationOrThrow(Guid id)
    {
        var application = await _applicationRepository.GetLoanApplicationByIdAsync(id);
        
        if (application == null)
            throw new NotFoundException("Application not found");
        
        return application;
    }
}