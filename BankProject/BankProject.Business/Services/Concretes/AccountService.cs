using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Concretes;

public class AccountService : IAccountService
{
    public const decimal LimitPerInternalTransfer = 10000;
    public const decimal LimitPerExternalTransfer = 8000;
    public const decimal DailyInternalTransferLimit = 30000;
    public const decimal DailyExternalTransferLimit = 24000;
    public const decimal LimitPerDepositAndWithdraw = 20000;

    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRecordRepository _transactionRecordRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly ITransactionApplicationRepository _transactionApplicationRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SemaphoreSlim _semaphoreSlim;

    public AccountService(
        UserManager<User> userManager,
        IMapper mapper,
        IUnitOfWork unitOfWork, 
        SemaphoreSlim semaphoreSlim,
        IAccountRepository accountRepository,
        ITransactionRecordRepository transactionRecordRepository,
        ITransactionApplicationRepository transactionApplicationRepository, 
        ILoanRepository loanRepository)
    {
        _accountRepository = accountRepository;
        _transactionRecordRepository = transactionRecordRepository;
        
        _transactionApplicationRepository = transactionApplicationRepository;
        _loanRepository = loanRepository;

        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _semaphoreSlim = semaphoreSlim;
    }
    
    public async Task<decimal> GetBalanceByAccountIdAsync(Guid id)
    {
        var account = await ValidateAndGetAccount(id);

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

    public async Task UpdateBalanceByAccountIdAsync(Guid id, decimal balance)
    {
        if (balance < 0)
            throw new InvalidOperationException("Balance must be greater than zero");
        
        var account = await ValidateAndGetAccount(id);

        await _accountRepository.UpdateBalanceByAccountIdAsync(account, balance);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        await PerformTransactionAsync(accountId, amount, TransactionType.Deposit);
    }

    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        await PerformTransactionAsync(accountId, amount, TransactionType.Withdraw);
    }

    public async Task InternalTransferAsync(Guid senderId, Guid receiverId, decimal amount)
    {
        await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.InternalTransfer);
    }

    public async Task ExternalTransferAsync(Guid senderId, Guid receiverId, decimal amount)
    {
        await UpdateAccountsAndCreateTransferTransaction(senderId, receiverId, amount, TransactionType.ExternalTransfer);
    }

    public async Task MakeBillPayment(Guid id,decimal amount)
    {
        var account = await _accountRepository.GetOrThrowNotFoundByIdAsync(id);

        ValidateAccountBalance(account,amount);

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

    public async Task MakeLoanPaymentAsync(Guid accountId, Guid loanId, decimal paymentAmount)
    {
        var loan = await _loanRepository.GetOrThrowNotFoundByIdAsync(loanId);
        ValidateLoanPayment(loan,paymentAmount);

        var account = await _accountRepository.GetOrThrowNotFoundByIdAsync(accountId);
        ValidateAccountForPayment(account,loan,paymentAmount);

        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            
            account.Balance -= paymentAmount;
            await _accountRepository.UpdateAsync(account.Id, account);

            UpdateLoanInformation(loan,paymentAmount);
            await _loanRepository.UpdateAsync(loan.Id, loan);
            
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    private static void ValidateLoanPayment(Loan loan, decimal paymentAmount)
    {
        if (loan.MonthlyPayment > paymentAmount)
            throw new Exception($"Payment amount is less than the scheduled payment amount of {loan.MonthlyPayment}");
        if (loan.RemainingDebt < paymentAmount)
            throw new Exception($"Payment amount is greater than the remaining debt of {loan.RemainingDebt}");
    }
    
    private static void ValidateAccountForPayment(Account account, Loan loan, decimal paymentAmount)
    {
        if (account.UserId != loan.UserId)
            throw new InvalidOperationException("User Id and Loan Id did not match!");
        if (account.Balance < paymentAmount)
            throw new InsufficientFundsException("Insufficient funds!");
    }
    
    private static void UpdateLoanInformation(Loan loan, decimal paymentAmount)
    {
        loan.RemainingDebt -= paymentAmount;
        if (DateTime.UtcNow <= loan.NextPaymentDueDate)
            loan.NumberOfTimelyPayments++;
        loan.NumberOfTotalPayments++;
        loan.LastPaymentDate = DateTime.UtcNow;
        loan.NextPaymentDueDate = loan.NextPaymentDueDate.AddMonths(1);
    }
    

    private async Task UpdateAccountsAndCreateTransferTransaction(Guid senderId, Guid receiverId, decimal amount,
        TransactionType transactionType)
    {
       ValidateTransferAmount(amount,transactionType);

       var senderAccount = await GetAndValidateSenderAccount(senderId, amount,transactionType);

       var receiverAccount = await ValidateAndGetAccount(receiverId);

        await ProcessTransfer(senderAccount, receiverAccount, amount, transactionType);
    }
    private static void ValidateTransferAmount(decimal amount, TransactionType transactionType)
    {
        var limit = transactionType == TransactionType.InternalTransfer ? LimitPerInternalTransfer : LimitPerExternalTransfer;
        var dailyLimit = transactionType == TransactionType.InternalTransfer ? DailyInternalTransferLimit : DailyExternalTransferLimit;

        if (amount > limit)
            throw new InvalidOperationException($"This operation exceeds the transfer limit of ${limit} per transfer.");
        if (amount > dailyLimit)
            throw new InvalidOperationException($"This operation exceeds the daily transfer limit of {dailyLimit}.");
    }
    
    private async Task<Account> GetAndValidateSenderAccount(Guid senderId, decimal amount, TransactionType transactionType)
    {
        var account = await ValidateAndGetAccount(senderId);

        ValidateAccountBalance(account,amount);
        
        var user = await _userManager.FindByIdAsync(account.UserId);
        if (user == null)
            throw new NotFoundException("Sender user not found!");
        var limit = transactionType == TransactionType.InternalTransfer
            ? DailyInternalTransferLimit
            : DailyExternalTransferLimit;
        if (limit < user.DailyTransferAmount + amount)
            throw new InvalidOperationException("Operation exceeds your daily transfer limit!");

        return account;
    }

    private static void ValidateAccountBalance(Account account,decimal amount)
    {
        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient funds!");
    }
    
    private async Task ProcessTransfer(Account senderAccount, Account receiverAccount, decimal amount, TransactionType transactionType)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await PerformAccountUpdate(senderAccount, receiverAccount, amount);
            await CreateTransactionRecord(senderAccount.Id, amount, transactionType, receiverAccount.Id);
            await UpdateUserDailyTransferAmount(senderAccount.UserId, amount);
            await _unitOfWork.TransactionCommitAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    
    private async Task PerformAccountUpdate(Account sender, Account receiver, decimal amount)
    {
        sender.Balance -= amount;
        receiver.Balance += amount;
        await _accountRepository.UpdateAsync(sender.Id, sender);
        await _accountRepository.UpdateAsync(receiver.Id, receiver);
    }
    
    private async Task UpdateUserDailyTransferAmount(string userId, decimal amount)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found!");
        user.DailyTransferAmount += amount;
        await _userManager.UpdateAsync(user);
    }
    
    
    private async Task PerformTransactionAsync(Guid accountId, decimal amount, TransactionType transactionType)
    {
        if (amount > LimitPerDepositAndWithdraw)
        {
            var transactionApplication = new TransactionApplication
            {
                AccountId = accountId,
                Amount = amount,
                CreatedDate = DateTime.UtcNow,
                TransactionType = transactionType
            };

            await _transactionApplicationRepository.CreateAsync(transactionApplication);
            await _unitOfWork.SaveChangesAsync();
            
            throw new InvalidOperationException($"this operation exceeds the daily transaction transfer limit of ${LimitPerDepositAndWithdraw}");
        }

        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null)
            throw new NotFoundException("Account not found!");
        
        var isCredit = transactionType == TransactionType.Deposit;
        var newBalance = isCredit ? account.Balance + amount : account.Balance - amount;
        
        if (newBalance < 0)
            throw new InsufficientFundsException("Insufficient funds");
        
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

    private async Task<Account> ValidateAndGetAccount(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null)
            throw new NotFoundException("Account not found!");

        return account;
    }

    private async Task CreateTransactionRecord(Guid accountId, decimal amount, TransactionType transactionType, Guid? receiverAccountId = null)
    {
        var transactionRecord = new TransactionRecord
        {
            Amount = amount,
            TransactionType = transactionType,
            AccountId = accountId,
            ReceiverAccountId = receiverAccountId
        };
        await _transactionRecordRepository.CreateAsync(transactionRecord);
    }
}