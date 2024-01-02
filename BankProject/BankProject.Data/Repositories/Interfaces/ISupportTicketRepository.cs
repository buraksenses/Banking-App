using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface ISupportTicketRepository : IReadRepository<SupportTicket,Guid>,ICreateRepository<SupportTicket,Guid>,IUpdateRepository<SupportTicket,Guid>
{
    
}