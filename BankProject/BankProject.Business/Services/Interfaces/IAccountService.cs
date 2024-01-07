using BankProject.Business.DTOs.Account;

namespace BankProject.Business.Services.Interfaces;

public interface IAccountService
{
   Task<decimal> GetBalanceByAccountIdAsync(Guid id);

   Task CreateAccountAsync(CreateAccountRequestDto requestDto);

   Task UpdateBalanceByAccountIdAsync(Guid id, decimal balance);

   Task DepositAsync(Guid accountId, decimal amount);

   Task WithdrawAsync(Guid accountId, decimal amount);

   Task InternalTransferAsync(Guid senderId, Guid receiverId, decimal amount);

   Task ExternalTransferAsync(Guid senderId, Guid receiverId, decimal amount);

   Task MakeBillPayment(Guid id, decimal amount);

   Task MakeLoanPaymentAsync(Guid accountId, Guid loanId, decimal paymentAmount);
}