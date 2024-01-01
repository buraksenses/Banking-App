using System.Linq.Expressions;
using BankProject.Core.Enums;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly CreateRepository<LoanApplication, Guid> _createRepository;
    private readonly ReadRepository<LoanApplication, Guid> _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoanApplicationRepository(IUnitOfWork unitOfWork)
    {
        _createRepository = unitOfWork.GetRepository<CreateRepository<LoanApplication, Guid>, LoanApplication, Guid>();
        _readRepository = unitOfWork.GetRepository<ReadRepository<LoanApplication, Guid>, LoanApplication, Guid>();
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateLoanApplicationStatusAsync(LoanApplication application, LoanApplicationStatus status)
    {
        application.LoanApplicationStatus = status;
        await _unitOfWork.CommitAsync();
    }

    public async Task CreateAsync(LoanApplication entity)
    {
        await _createRepository.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<LoanApplication?> GetByIdAsync(Guid id)
    {
        return await _readRepository.GetByIdAsync(id);
    }

    public async Task<LoanApplication?> GetByIdAsync(Expression<Func<LoanApplication, bool>> predicate)
    {
        return await _readRepository.GetByIdAsync(predicate);
    }

    public async Task<List<LoanApplication>> GetAllAsync(Expression<Func<LoanApplication, bool>> predicate)
    {
        return await _readRepository.GetAllAsync(predicate);
    }

    public async Task<List<LoanApplication>> GetAllAsync()
    {
        return await _readRepository.GetAllAsync();
    }
}