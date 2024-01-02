namespace BankProject.Business.DTOs.SupportTicket;

public class CreateSupportTicketRequestDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string UserId { get; set; }
}