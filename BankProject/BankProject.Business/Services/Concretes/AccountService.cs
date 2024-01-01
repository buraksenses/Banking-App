using System.Linq.Expressions;
using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using Transaction = BankProject.Data.Entities.Transaction;

namespace BankProject.Business.Services.Concretes;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public AccountService(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        UserManager<User> userManager,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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
        await PerformTransactionAsync(accountId, amount, TransactionType.Deposit);
    }

    public async Task WithdrawAsync(Guid accountId, float amount)
    {
        await PerformTransactionAsync(accountId, amount, TransactionType.Withdraw);
    }

    public async Task InternalTransferAsync(Guid senderId, Guid receiverId, float amount)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.InternalTransfer);
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
            await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.ExternalTransfer);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task MakePayment(Guid id,float amount)
    {
        var account = await GetAccountOrThrow(account => account.Balance > amount && account.Id == id);

        await _semaphoreSlim.WaitAsync();
        try
        {
            await UpdateAccountBalance(account, amount, false);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    
    private async Task UpdateAccountsAndCreateTransferTransaction(Guid senderId, Guid receiverId, float amount,
        TransactionType transactionType)
    {
        var senderAccount = await GetAccountOrThrow(account => account.Id == senderId && account.Balance > amount);
        
        var receiverAccount = await GetAccountOrThrow(receiverId);

        await _unitOfWork.BeginTransactionAsync();

        await UpdateAccountBalance(senderAccount, amount, false);
        await UpdateAccountBalance(receiverAccount, amount, true);
        
        await _unitOfWork.TransactionCommitAsync();

        await CreateTransactionRecord(senderId, amount, transactionType, receiverId);
    }
    
    private async Task PerformTransactionAsync(Guid accountId, float amount, TransactionType transactionType)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var account = await GetAccountOrThrow(accountId);
            
            var isCredit = transactionType == TransactionType.Deposit;
            var newBalance = isCredit ? account.Balance + amount : account.Balance - amount;

            if (newBalance < 0)
                throw new InvalidOperationException("Insufficient funds");

            await _unitOfWork.BeginTransactionAsync();
            await UpdateAccountBalance(account, newBalance,isCredit);
            await CreateTransactionRecord(accountId, amount, transactionType);
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<Account> GetAccountOrThrow(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        
        if (account == null)
            throw new NotFoundException("Account not found");
        
        return account;
    }

    public async Task<Account> GetAccountOrThrow(Expression<Func<Account, bool>> predicate)
    {
        var account = await _accountRepository.GetByIdAsync(predicate);

        if (account == null)
            throw new NotFoundException("Account not found or Insufficient funds!");

        return account;
    }
    
    private async Task UpdateAccountBalance(Account account, float amount, bool isCredit)
    {
        account.Balance += isCredit ? amount : -amount;
        await _accountRepository.UpdateBalanceByAccountIdAsync(account.Id, account.Balance);
    }
    
    private async Task CreateTransactionRecord(Guid accountId, float amount, TransactionType transactionType, Guid? receiverAccountId = null)
    {
        var transactionRecord = new Transaction
        {
            Amount = amount,
            TransactionType = transactionType,
            AccountId = accountId,
            ReceiverAccountId = receiverAccountId
        };
        await _transactionRepository.CreateAsync(transactionRecord);
    }
}