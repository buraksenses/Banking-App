namespace BankProject.Business.DTOs.Account;

public class DepositDto
{
    public Guid AccountId { get; set; }

    public float Amount { get; set; }
}