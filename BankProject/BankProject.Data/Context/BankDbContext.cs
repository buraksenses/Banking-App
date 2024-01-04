using BankProject.Data.Entities;
using BankProject.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public class BankDbContext : IdentityDbContext<User>
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Seed();

        modelBuilder.ApplyEnumConversions();
    }

    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<TransactionRecord> Transactions { get; set; }
    
    public DbSet<Loan> Loans { get; set; }

    public DbSet<LoanApplication> LoanApplications { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<SupportTicket> SupportTickets { get; set; }

    public DbSet<TransactionApplication> TransactionApplications { get; set; }
}