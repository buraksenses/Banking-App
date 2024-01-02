using System.Linq.Expressions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly CreateRepository<SupportTicket, Guid> _createRepository;
    private readonly ReadRepository<SupportTicket, Guid> _readRepository;
    private readonly UpdateRepository<SupportTicket, Guid> _updateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SupportTicketRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<SupportTicket, Guid>, SupportTicket, Guid>();
        _readRepository = unitOfWork.GetRepository<ReadRepository<SupportTicket, Guid>, SupportTicket, Guid>();
        _updateRepository = unitOfWork.GetRepository<UpdateRepository<SupportTicket, Guid>, SupportTicket, Guid>();
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        return await _readRepository.GetByIdAsync(id);
    }

    public async Task<SupportTicket?> GetByIdAsync(Expression<Func<SupportTicket, bool>> predicate)
    {
        return await _readRepository.GetByIdAsync(predicate);
    }

    public async Task<List<SupportTicket>> GetAllAsync(Expression<Func<SupportTicket, bool>> predicate)
    {
        return await _readRepository.GetAllAsync(predicate);
    }

    public async Task<List<SupportTicket>> GetAllAsync()
    {
        return await _readRepository.GetAllAsync();
    }

    public async Task CreateAsync(SupportTicket entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(Guid id, SupportTicket entity)
    {
        await _updateRepository.UpdateAsync(id, entity);
        await _unitOfWork.CommitAsync();
    }
}