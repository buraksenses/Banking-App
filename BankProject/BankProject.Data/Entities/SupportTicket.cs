using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class SupportTicket : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public TicketPriority TicketPriority { get; set; }

    public TicketStatus TicketStatus { get; set; }

    public string UserId { get; set; }
    
    //Navigation Properties
    public User User { get; set; }
}