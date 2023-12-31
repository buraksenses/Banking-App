using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class LoanRepository : ILoanRepository
{
    private readonly CreateRepository<Loan, Guid> _createRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoanRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<Loan, Guid>, Loan, Guid>();
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(Loan entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }
}