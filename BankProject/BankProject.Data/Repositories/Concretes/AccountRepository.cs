using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class AccountRepository : IAccountRepository
{
    private readonly IGenericRepository<Account, Guid> _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountRepository(IUnitOfWork unitOfWork)
    {
        _accountRepository = unitOfWork.Repository<Account, Guid>();
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(Account entity)
    {
        await _accountRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }

    public async Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            return null;
        
        account.Balance = balance;
        await _unitOfWork.CommitAsync();
        
        return account;
    }
}