using AutoMapper;
using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Constants;
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

        await _applicationRepository.CreateAsync(loanApplication);
    }

    public async Task<GetLoanApplicationRequestDto> GetLoanApplicationByIdAsync(Guid applicationId)
    {
        var application = await GetLoanApplicationOrThrow(applicationId);

        var applicationDto = _mapper.Map<GetLoanApplicationRequestDto>(application);

        return applicationDto;
    }

    public async Task<LoanApplicationResponseDto> GetRecommendationForApplicationByIdAsync(Guid applicationId)
    {
        var application = await GetLoanApplicationOrThrow(applicationId);

        var user = await _userManager.FindByIdAsync(application.UserId);

        if (user == null)
            throw new NotFoundException("User not found!");

        var creditScore = _creditScoreService.CalculateCreditScore(user);

        var minimumRequiredScore =
            _creditScoreService.CalculateMinimumRequiredCreditScoreForLoanApplication(application);

        return new LoanApplicationResponseDto
        {
            UserCreditScore = creditScore,
            MinimumRequiredCreditScoreForApplication = minimumRequiredScore,
            Recommendation = creditScore > minimumRequiredScore
                ? CreditScoreConstants.PositiveApplicationResponse
                : CreditScoreConstants.NegativeApplicationResponse
        };
    }

    public async Task<CreateLoanRequestDto> ApproveLoanApplicationByIdAndCreateLoanAsync(Guid applicationId)
    {
        var application = await GetLoanApplicationOrThrow(applicationId);

        await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Approved);

        var loan = _mapper.Map<Loan>(application);
        
        await _loanService.CreateLoanAsync(loan);

        var loanDto = _mapper.Map<CreateLoanRequestDto>(loan);

        return loanDto;
    }

    public async Task<GetLoanApplicationRequestDto> RejectLoanApplicationByIdAsync(Guid applicationId)
    {
        var application = await GetLoanApplicationOrThrow(applicationId);

        await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Rejected);

        var applicationDto = _mapper.Map<GetLoanApplicationRequestDto>(application);

        return applicationDto;
    }


    private async Task<LoanApplication> GetLoanApplicationOrThrow(Guid id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        
        if (application == null)
            throw new NotFoundException("Application not found");
        
        return application;
    }
}