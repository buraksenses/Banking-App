using BankProject.Business.DTOs.Account;

namespace BankProject.Business.Services.Interfaces;

public interface IAccountService
{
    public Task<float> GetBalanceByAccountIdAsync(Guid id);

    public Task CreateAccountAsync(CreateAccountRequestDto requestDto);

    public Task UpdateBalanceByAccountIdAsync(Guid id, float balance);
}