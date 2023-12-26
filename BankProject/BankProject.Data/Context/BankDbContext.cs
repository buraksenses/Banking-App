using BankProject.Core.Enums;
using BankProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccountType)
                .HasConversion(
                    v => v.ToString(),  
                    v => (AccountType)Enum.Parse(typeof(AccountType), v));
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name)
                .HasConversion(v => v.ToString(), 
                    v => (RoleType)Enum.Parse(typeof(RoleType), v));
        });
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Role> Roles { get; set; }
    
    public DbSet<User> Users { get; set; }
}