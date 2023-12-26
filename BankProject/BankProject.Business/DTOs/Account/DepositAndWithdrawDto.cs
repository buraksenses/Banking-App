namespace BankProject.Business.DTOs.Account;

public class DepositAndWithdrawDto
{
    public Guid AccountId { get; set; }

    public float Amount { get; set; }
}