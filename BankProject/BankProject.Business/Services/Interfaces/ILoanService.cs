using BankProject.Data.Entities;

namespace BankProject.Business.Services.Interfaces;

public interface ILoanService
{
    Task CreateLoanAsync(Loan loan);
}