using System.Linq.Expressions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class AccountRepository : IAccountRepository
{
    private readonly CreateRepository<Account, Guid> _createRepository;
    private readonly ReadRepository<Account, Guid> _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<Account, Guid>, Account, Guid>();
        _readRepository = unitOfWork.GetRepository<ReadRepository<Account, Guid>, Account, Guid>();
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(Account entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _readRepository.GetByIdAsync(id);
    }

    public Task<List<Account>> GetAllAsync(Expression<Func<Account, bool>> predicate)
    {
        return _readRepository.GetAllAsync(predicate);
    }

    public async Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        var account = await _readRepository.GetByIdAsync(id);
        if (account == null)
            return null;
        
        account.Balance = balance;
        await _unitOfWork.CommitAsync();
        
        return account;
    }
}