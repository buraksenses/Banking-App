using System.Linq.Expressions;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class SupportTicketRepository : GenericRepository<SupportTicket,Guid>, ISupportTicketRepository
{
    public SupportTicketRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}