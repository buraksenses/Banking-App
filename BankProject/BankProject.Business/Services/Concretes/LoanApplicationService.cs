using AutoMapper;
using BankProject.Business.DTOs.Loan;
using BankProject.Business.Helpers;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Constants;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Concretes;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;
    private readonly ICreditScoreService _creditScoreService;
    private readonly UserManager<User> _userManager;
    private readonly ILoanService _loanService;
    private readonly IUnitOfWork _unitOfWork;

    public LoanApplicationService(
        IMapper mapper,
        ICreditScoreService creditScoreService,
        UserManager<User> userManager,
        ILoanService loanService,
        IUnitOfWork unitOfWork)
    {
        _applicationRepository = unitOfWork.GetRepository<LoanApplicationRepository, LoanApplication, Guid>();
        _mapper = mapper;
        _creditScoreService = creditScoreService;
        _userManager = userManager;
        _loanService = loanService;
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetLoanApplicationRequestDto> GetLoanApplicationByIdAsync(Guid applicationId)
    {
        var application = await _applicationRepository.GetOrThrowAsync(applicationId);

        var applicationDto = _mapper.Map<GetLoanApplicationRequestDto>(application);

        return applicationDto;
    }

    public async Task<LoanApplicationResponseDto> GetRecommendationForApplicationByIdAsync(Guid applicationId)
    {
        var application = await _applicationRepository.GetOrThrowAsync(applicationId);

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
        var application = await _applicationRepository.GetOrThrowAsync(applicationId);

        await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Approved);

        var loan = _mapper.Map<Loan>(application);
        
        await _loanService.CreateLoanAsync(loan);

        var loanDto = _mapper.Map<CreateLoanRequestDto>(loan);

        await _unitOfWork.SaveChangesAsync();

        return loanDto;
    }

    public async Task<GetLoanApplicationRequestDto> RejectLoanApplicationByIdAsync(Guid applicationId)
    {
        var application = await _applicationRepository.GetOrThrowAsync(applicationId);

        await _applicationRepository.UpdateLoanApplicationStatusAsync(application, LoanApplicationStatus.Rejected);

        var applicationDto = _mapper.Map<GetLoanApplicationRequestDto>(application);

        await _unitOfWork.SaveChangesAsync();

        return applicationDto;
    }
}