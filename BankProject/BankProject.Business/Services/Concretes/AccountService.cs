using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<float> GetBalanceByAccountIdAsync(Guid id)
    {
        return await _repository.GetBalanceByAccountIdAsync(id);
    }

    public async Task CreateAccountAsync(CreateAccountRequestDto requestDto)
    {
        var account = _mapper.Map<Account>(requestDto);
        
        //User var mi kontrol et.

        await _repository.CreateAccountAsync(account);
    }

    public async Task UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        await _repository.UpdateBalanceByAccountIdAsync(id, balance);
    }
}