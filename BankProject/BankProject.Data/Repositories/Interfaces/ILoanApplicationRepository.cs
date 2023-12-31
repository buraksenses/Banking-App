using BankProject.Core.Enums;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface ILoanApplicationRepository : ICreateRepository<LoanApplication,Guid>,IReadRepository<LoanApplication,Guid>
{
    Task UpdateLoanApplicationStatusAsync(LoanApplication application, LoanApplicationStatus status);
}