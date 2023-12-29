using BankProject.Business.DTOs.Loan;

namespace BankProject.Business.Services.Interfaces;

public interface ILoanApplicationService
{
    Task CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto);

    Task<GetLoanApplicationRequestDto> GetLoanApplicationByIdAsync(Guid id);
}