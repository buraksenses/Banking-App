using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Business.Services.Concretes;

public class TransactionApplicationService : ITransactionApplicationService
{
    private readonly ITransactionApplicationRepository _applicationRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionApplicationService(
        SemaphoreSlim semaphoreSlim,
        IUnitOfWork unitOfWork, 
        ITransactionApplicationRepository applicationRepository, 
        IAccountRepository accountRepository)
    {
        _semaphoreSlim = semaphoreSlim;
        _unitOfWork = unitOfWork;
        _applicationRepository = applicationRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task ApproveApplicationAsync(Guid id)
    {
        var application = await _applicationRepository.GetOrThrowNotFoundByIdAsync(id);
        if (application.Status != TransactionApplicationStatus.Pending)
            throw new InvalidOperationException("Application is not in pending status.");

        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var account = await _accountRepository.GetOrThrowNotFoundByIdAsync(application.AccountId);
            await _accountRepository.UpdateBalanceByAccountIdAsync(account,account.Balance - application.Amount);
            application.Status = TransactionApplicationStatus.Approved;
            await _applicationRepository.UpdateAsync(id, application);
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task RejectApplicationAsync(Guid id)
    {
        var application = await _applicationRepository.GetOrThrowNotFoundByIdAsync(id);
        if (application.Status != TransactionApplicationStatus.Pending)
            throw new InvalidOperationException("Application is not in pending status.");

        application.Status = TransactionApplicationStatus.Rejected;
        await _applicationRepository.UpdateAsync(id, application);
    }
}