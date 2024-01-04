using BankProject.Core.Enums;
using BankProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
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
                NormalizedName = "ADMIN"
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
                NormalizedName = "BANK_OFFICER"
            },
            new IdentityRole
            {
                Id = advisorRoleId,
                ConcurrencyStamp = advisorRoleId,
                Name = "Advisor",
                NormalizedName = "ADVISOR"
            },
            new IdentityRole
            {
                Id = loanOfficerRoleId,
                ConcurrencyStamp = loanOfficerRoleId,
                Name = "Loan_Officer",
                NormalizedName = "LOAN_OFFICER"
            },
            new IdentityRole
            {
                Id = auditorRoleId,
                ConcurrencyStamp = auditorRoleId,
                Name = "Auditor",
                NormalizedName = "AUDITOR"
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }

    public static void ApplyEnumConversions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccountType)
                .HasConversion(
                    v => v.ToString(),  
                    v => (AccountType)Enum.Parse(typeof(AccountType), v));
        });

        modelBuilder.Entity<TransactionRecord>(entity =>
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
        
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.Property(e => e.TicketPriority)
                .HasConversion(v => v.ToString(), 
                    v => (TicketPriority)Enum.Parse(typeof(TicketPriority), v));
        });
        
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.Property(e => e.TicketStatus)
                .HasConversion(v => v.ToString(), 
                    v => (TicketStatus)Enum.Parse(typeof(TicketStatus), v));
        });
    }
}