using BankProject.Business.Services.Interfaces;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Business.Services.Concretes;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public LoanService(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.GetRepository<LoanRepository, Loan, Guid>();
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateLoanAsync(Loan loan)
    {
        await _repository.CreateAsync(loan);
    }
}