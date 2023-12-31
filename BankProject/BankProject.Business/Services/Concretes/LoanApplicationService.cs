using AutoMapper;
using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Concretes;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;
    private readonly ICreditScoreService _creditScoreService;
    private readonly UserManager<User> _userManager;
    private readonly ILoanService _loanService;

    public LoanApplicationService(ILoanApplicationRepository applicationRepository,
        IMapper mapper,
        ICreditScoreService creditScoreService,
        UserManager<User> userManager,
        ILoanService loanService)
    {
        _applicationRepository = applicationRepository;
        _mapper = mapper;
        _creditScoreService = creditScoreService;
        _userManager = userManager;
        _loanService = loanService;
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

    public async Task ProcessLoanApplicationStatusAsync(Guid applicationId)
    {
        var application = await GetLoanApplicationOrThrow(applicationId);

        var user = await _userManager.FindByIdAsync(application.UserId);

        if (user == null)
            throw new NotFoundException("User not found!");

        var creditScore = _creditScoreService.CalculateCreditScore(user);

        var minimumRequiredScore =
            _creditScoreService.CalculateMinimumRequiredCreditScoreForLoanApplication(application);

        if (creditScore > minimumRequiredScore)
        {
            var loan = _mapper.Map<Loan>(application);

            loan.RemainingDebt = application.LoanAmount;

            await _loanService.CreateLoanAsync(loan);

            await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Approved);

            return;
        }

        await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Rejected);
    }

    private async Task<LoanApplication> GetLoanApplicationOrThrow(Guid id)
    {
        var application = await _applicationRepository.GetLoanApplicationByIdAsync(id);
        
        if (application == null)
            throw new NotFoundException("Application not found");
        
        return application;
    }
}