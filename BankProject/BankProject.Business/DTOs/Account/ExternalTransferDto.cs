namespace BankProject.Business.DTOs.Account;

public class ExternalTransferDto
{
    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public float Amount { get; set; }
}