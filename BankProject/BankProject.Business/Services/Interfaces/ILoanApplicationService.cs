using BankProject.Business.DTOs.Loan;

namespace BankProject.Business.Services.Interfaces;

public interface ILoanApplicationService
{
    Task CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto);

    Task<GetLoanApplicationRequestDto> GetLoanApplicationByIdAsync(Guid applicationId);

    Task<LoanApplicationResponseDto> GetRecommendationForApplicationByIdAsync(Guid applicationId);

    Task<CreateLoanRequestDto> ApproveLoanApplicationByIdAndCreateLoanAsync(Guid applicationId);

    Task<GetLoanApplicationRequestDto> RejectLoanApplicationByIdAsync(Guid applicationId);
}