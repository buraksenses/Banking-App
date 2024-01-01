using System.Linq.Expressions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionRepository : ITransactionRepository
{
    private readonly CreateRepository<Transaction, Guid> _createRepository;
    private readonly ReadRepository<Transaction, Guid> _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<Transaction, Guid>, Transaction, Guid>();
        _readRepository = unitOfWork.GetRepository<ReadRepository<Transaction, Guid>, Transaction, Guid>();
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateAsync(Transaction entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _readRepository.GetByIdAsync(id);
    }

    public async Task<Transaction?> GetByIdAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _readRepository.GetByIdAsync(predicate);
    }

    public async Task<List<Transaction>> GetAllAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _readRepository.GetAllAsync(predicate);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _readRepository.GetAllAsync();
    }
}