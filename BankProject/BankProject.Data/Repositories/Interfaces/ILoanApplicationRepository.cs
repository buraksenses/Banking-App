using BankProject.Core.Enums;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface ILoanApplicationRepository : IGenericRepository<LoanApplication,Guid>
{
    Task UpdateLoanApplicationStatusAsync(LoanApplication application, LoanApplicationStatus status);
}