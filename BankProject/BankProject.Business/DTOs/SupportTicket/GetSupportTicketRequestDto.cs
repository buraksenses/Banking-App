using BankProject.Core.Enums;

namespace BankProject.Business.DTOs.SupportTicket;

public class GetSupportTicketRequestDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public TicketPriority TicketPriority { get; set; }

    public TicketStatus TicketStatus { get; set; }

    public string UserId { get; set; }
}