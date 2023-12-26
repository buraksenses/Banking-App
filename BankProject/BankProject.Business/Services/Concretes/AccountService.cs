using System.Transactions;
using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Transaction = BankProject.Data.Entities.Transaction;

namespace BankProject.Business.Services.Concretes;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
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

        var user = await _userRepository.GetByIdAsync(account.UserId);

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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var account = await GetAccountOrThrow(accountId);

            account.Balance += amount;
            await _accountRepository.UpdateBalanceByAccountIdAsync(accountId,account.Balance);

            var transactionRecord = new Transaction
            {
                Amount = amount,
                TransactionType = TransactionType.Deposit,
                AccountId = accountId
            };
            await _transactionRepository.CreateAsync(transactionRecord);

            scope.Complete();
        }
    }

    private async Task<Account> GetAccountOrThrow(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        
        if (account == null)
        {
            throw new NotFoundException("Account not found");
        }

        return account;
    }
}