using BankProject.Business.DTOs.Loan;

namespace BankProject.Business.Services.Interfaces;

public interface ILoanService
{
    Task CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto);
}