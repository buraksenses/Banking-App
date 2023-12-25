using BankProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
        
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Role> Roles { get; set; }
    
    public DbSet<User> Users { get; set; }
}