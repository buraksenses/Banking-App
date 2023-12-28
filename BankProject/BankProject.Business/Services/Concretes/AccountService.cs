using System.Transactions;
using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Transaction = BankProject.Data.Entities.Transaction;

namespace BankProject.Business.Services.Concretes;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public AccountService(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<float> GetBalanceByAccountIdAsync(Guid id)
    {
        var account = await GetAccountOrThrow(id);

        return account.Balance;
    }

    public async Task CreateAccountAsync(CreateAccountRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.AccountType, true, out AccountType accountType))
        {
            throw new ArgumentException("Invalid account type.");
        }
        
        requestDto.AccountType = accountType.ToString();
        
        var account = _mapper.Map<Account>(requestDto);

        var user = await _userManager.FindByIdAsync(account.UserId);

        if (user == null)
            throw new NotFoundException("User not found!");

        await _accountRepository.CreateAsync(account);
    }

    public async Task UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        await GetAccountOrThrow(id);

        await _accountRepository.UpdateBalanceByAccountIdAsync(id, balance);
    }

    public async Task DepositAsync(Guid accountId, float amount)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await UpdateAccountAndCreateAccountTransaction(accountId, amount, TransactionType.Deposit);
                scope.Complete();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task WithdrawAsync(Guid accountId, float amount)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await UpdateAccountAndCreateAccountTransaction(accountId, amount, TransactionType.Withdraw);
                scope.Complete();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task InternalTransferAsync(Guid senderId, Guid receiverId, float amount)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.InternalTransfer);
            
                scope.Complete();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
        
    }

    public async Task ExternalTransferAsync(Guid senderId, Guid receiverId, float amount)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.ExternalTransfer);
            
                scope.Complete();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    
    private async Task UpdateAccountsAndCreateTransferTransaction(Guid senderId, Guid receiverId, float amount,
        TransactionType transactionType)
    {
        var senderAccount = await GetAccountOrThrow(senderId);
        var receiverAccount = await GetAccountOrThrow(receiverId);

        //Where koy db yapsin
        if (senderAccount.Balance < amount)
            throw new Exception("Insufficient funds");

        senderAccount.Balance -= amount;
        receiverAccount.Balance += amount;

        await _accountRepository.UpdateBalanceByAccountIdAsync(senderId, senderAccount.Balance);
        await _accountRepository.UpdateBalanceByAccountIdAsync(receiverId, receiverAccount.Balance);

        var transactionRecord = new Transaction
        {
            Amount = amount,
            TransactionType = transactionType == TransactionType.InternalTransfer ? TransactionType.InternalTransfer : TransactionType.ExternalTransfer,
            AccountId = senderId,
            ReceiverAccountId = receiverId
        };

        await _transactionRepository.CreateAsync(transactionRecord);
    }
    
    private async Task UpdateAccountAndCreateAccountTransaction(Guid accountId, float amount, TransactionType transactionType)
    {
        var account = await GetAccountOrThrow(accountId);

        switch (transactionType)
        {
            case TransactionType.Deposit:
                account.Balance += amount;
                break;
            case TransactionType.Withdraw:
                account.Balance -= amount; 
                break;
        }

        await _accountRepository.UpdateBalanceByAccountIdAsync(accountId, account.Balance);

        var transactionRecord = new Transaction
        {
            Amount = amount,
            TransactionType = transactionType,
            AccountId = accountId
        };
        await _transactionRepository.CreateAsync(transactionRecord);
    }
    
    private async Task<Account> GetAccountOrThrow(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        
        if (account == null)
            throw new NotFoundException("Account not found");
        
        return account;
    }
}