using BankProject.Core.Enums;
using BankProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Context;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.NewGuid(), Name = RoleType.ADMIN },
            new Role { Id = Guid.NewGuid(), Name = RoleType.CUSTOMER },
            new Role { Id = Guid.NewGuid(), Name = RoleType.BANK_OFFICER },
            new Role { Id = Guid.NewGuid(), Name = RoleType.ADVISOR },
            new Role { Id = Guid.NewGuid(), Name = RoleType.LOAN_OFFICER },
            new Role { Id = Guid.NewGuid(), Name = RoleType.AUDITOR }
        );
    }
}