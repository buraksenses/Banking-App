using System.Linq.Expressions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class PaymentRepository : IPaymentRepository
{
    private readonly CreateRepository<Payment, Guid> _createRepository;
    private readonly ReadRepository<Payment, Guid> _readRepository;
    private readonly UpdateRepository<Payment, Guid> _updateRepository;
    private readonly DeleteRepository<Payment, Guid> _deleteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<Payment, Guid>, Payment, Guid>();
        _readRepository = unitOfWork.GetRepository<ReadRepository<Payment, Guid>, Payment, Guid>();
        _updateRepository = unitOfWork.GetRepository<UpdateRepository<Payment, Guid>, Payment, Guid>();
        _deleteRepository = unitOfWork.GetRepository<DeleteRepository<Payment, Guid>, Payment, Guid>();
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateAsync(Payment entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _readRepository.GetByIdAsync(id);
    }

    public async Task<Payment?> GetByIdAsync(Expression<Func<Payment, bool>> predicate)
    {
        return await _readRepository.GetByIdAsync(predicate);
    }

    public async Task<List<Payment>> GetAllAsync(Expression<Func<Payment, bool>> predicate)
    {
        return await _readRepository.GetAllAsync(predicate);
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _readRepository.GetAllAsync();
    }

    public async Task UpdateAsync(Guid id, Payment entity)
    {
        await _updateRepository.UpdateAsync(id, entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _deleteRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }
}