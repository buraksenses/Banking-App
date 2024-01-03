﻿using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Helpers;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
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
    private readonly SemaphoreSlim _semaphoreSlim;

    public AccountService(
        UserManager<User> userManager,
        IMapper mapper,
        IUnitOfWork unitOfWork, 
        SemaphoreSlim semaphoreSlim)
    {
        _accountRepository = unitOfWork.GetRepository<AccountRepository, Account, Guid>();
        _transactionRepository = unitOfWork.GetRepository<TransactionRepository, Transaction, Guid>();
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _semaphoreSlim = semaphoreSlim;
    }
    
    public async Task<float> GetBalanceByAccountIdAsync(Guid id)
    {
        var account = await _accountRepository.GetOrThrowAsync(id);

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

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        var account =  await _accountRepository.GetOrThrowAsync(id);

        await _accountRepository.UpdateBalanceByAccountIdAsync(account, balance);

        await _unitOfWork.SaveChangesAsync();
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
        await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.InternalTransfer);
    }

    public async Task ExternalTransferAsync(Guid senderId, Guid receiverId, float amount)
    {
        await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.ExternalTransfer);
    }

    public async Task MakePayment(Guid id,float amount)
    {
        var account = await _accountRepository.GetOrThrowAsync(account => account.Balance > amount && account.Id == id);

        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.UpdateBalanceByAccountIdAsync(account, account.Balance - amount);
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task UpdateAccountsAndCreateTransferTransaction(Guid senderId, Guid receiverId, float amount,
        TransactionType transactionType)
    {
        var senderAccount = await _accountRepository.GetOrThrowAsync(account => account.Id == senderId && account.Balance > amount);

        var user = await _userManager.FindByIdAsync(senderAccount.UserId);
        if (user == null)
            throw new NotFoundException("Sender user not found!");
        
        if (user.DailyTransferLimit < user.DailyTransferAmount + (decimal)amount)
            throw new InvalidOperationException("Operation exceeds your daily transfer limit!");
        
        var receiverAccount = await _accountRepository.GetOrThrowAsync(receiverId);
        
        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.UpdateBalanceByAccountIdAsync(senderAccount, senderAccount.Balance - amount);
            await _accountRepository.UpdateBalanceByAccountIdAsync(receiverAccount, receiverAccount.Balance + amount);
            await CreateTransactionRecord(senderId, amount, transactionType, receiverId);
            user.DailyTransferAmount += (decimal)amount;
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    
    private async Task PerformTransactionAsync(Guid accountId, float amount, TransactionType transactionType)
    {
        var account = await _accountRepository.GetOrThrowAsync(accountId);
        
        var isCredit = transactionType == TransactionType.Deposit;
        var newBalance = isCredit ? account.Balance + amount : account.Balance - amount;
        
        if (newBalance < 0)
            throw new InvalidOperationException("Insufficient funds");
        
        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.UpdateBalanceByAccountIdAsync(account, newBalance);
            await CreateTransactionRecord(accountId, amount, transactionType);
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
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