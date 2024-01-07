namespace BankProject.Business.DTOs.Account;

public class InternalTransferDto
{
    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public decimal Amount { get; set; }
}