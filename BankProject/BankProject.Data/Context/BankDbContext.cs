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
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}