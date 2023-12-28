using BankProject.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public class BankAuthDbContext : IdentityDbContext<User>
{
    public BankAuthDbContext(DbContextOptions<BankAuthDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Seed();
    }
}