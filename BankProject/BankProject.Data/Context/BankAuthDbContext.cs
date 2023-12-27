using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public class BankAuthDbContext : IdentityDbContext
{
    public BankAuthDbContext(DbContextOptions<BankAuthDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        const string adminRoleId = "50df0d64-b2ae-482a-98d1-bd187fbbbeda";
        const string customerRoleId = "ab24b60e-d36f-4662-ab2c-0faefdb86d3e";
        const string bankOfficerRoleId = "8ab640f3-91db-417b-a3dd-024b14837f96";
        const string advisorRoleId = "efd72960-b554-48b5-88c6-370fca080035";
        const string loanOfficerRoleId = "df167571-d103-4488-b398-52f81c2f2fbd";
        const string auditorRoleId = "6d6eb8c4-151c-45ec-9083-9d8877852e78";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId,
                Name = "Admin",
                NormalizedName = "Admin".ToUpper()
            },
            new IdentityRole
            {
                Id = customerRoleId,
                ConcurrencyStamp = customerRoleId,
                Name = "Customer",
                NormalizedName = "Customer".ToUpper()
            },
            new IdentityRole
            {
                Id = bankOfficerRoleId,
                ConcurrencyStamp = bankOfficerRoleId,
                Name = "Bank_Officer",
                NormalizedName = "Bank_Officer".ToUpper()
            },
            new IdentityRole
            {
                Id = advisorRoleId,
                ConcurrencyStamp = advisorRoleId,
                Name = "Advisor",
                NormalizedName = "Advisor".ToUpper()
            },
            new IdentityRole
            {
                Id = loanOfficerRoleId,
                ConcurrencyStamp = loanOfficerRoleId,
                Name = "Loan_Officer",
                NormalizedName = "Loan_Officer".ToUpper()
            },
            new IdentityRole
            {
                Id = auditorRoleId,
                ConcurrencyStamp = auditorRoleId,
                Name = "Auditor",
                NormalizedName = "Auditor".ToUpper()
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}