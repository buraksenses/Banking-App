using BankProject.Core.Enums;
using BankProject.Data.Entities;
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

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccountType)
                .HasConversion(
                    v => v.ToString(),  
                    v => (AccountType)Enum.Parse(typeof(AccountType), v));
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.TransactionType)
                .HasConversion(v => v.ToString(), 
                    v => (TransactionType)Enum.Parse(typeof(TransactionType), v));
        });
        
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.LoanType)
                .HasConversion(v => v.ToString(), 
                    v => (LoanType)Enum.Parse(typeof(LoanType), v));
        });
        
        modelBuilder.Entity<LoanApplication>(entity =>
        {
            entity.Property(e => e.LoanType)
                .HasConversion(v => v.ToString(), 
                    v => (LoanType)Enum.Parse(typeof(LoanType), v));
        });
        
        modelBuilder.Entity<LoanApplication>(entity =>
        {
            entity.Property(e => e.LoanApplicationStatus)
                .HasConversion(v => v.ToString(), 
                    v => (LoanApplicationStatus)Enum.Parse(typeof(LoanApplicationStatus), v));
        });
        
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.TimePeriod)
                .HasConversion(v => v.ToString(), 
                    v => (TimePeriod)Enum.Parse(typeof(TimePeriod), v));
        });
        
    }

    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    
    public DbSet<Loan> Loans { get; set; }

    public DbSet<LoanApplication> LoanApplications { get; set; }

    public DbSet<Payment> Payments { get; set; }
}