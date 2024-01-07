using BankProject.Core.Enums;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankProject.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public CustomWebApplicationFactory()
    {
        // Bu constructor içerisinde özel yapılandırmalar yapabilirsiniz.
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Mevcut DbContext konfigürasyonunu bul ve kaldır
            var descriptor = services.Single(
                d => d.ServiceType ==
                     typeof(DbContextOptions<BankDbContext>));

            services.Remove(descriptor);

            // In-memory veritabanını ekleyin
            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // ServiceProvider oluştur ve veritabanını başlat
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BankDbContext>();
                db.Database.EnsureCreated();

                // Test verileri ekleyin
                SeedDatabase(db);
            }
        });
    }

    private static void SeedDatabase(BankDbContext db)
    {
        for (int i = 0; i < 10; i++)
        {
            db.Accounts.Add(new Account
            {
                Balance = 3000 * i + 1,
                AccountType = AccountType.Deposit,
                UserId = i.ToString()
            });
        }

        for (int i = 0; i < 10; i++)
        {
            db.Users.Add(new User
            {
                Id = i.ToString(),
                UserName = "user" + i,
                NormalizedUserName = null,
                Email = null,
                NormalizedEmail = null,
                EmailConfirmed = false,
                PasswordHash = null,
                SecurityStamp = null,
                ConcurrencyStamp = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                AnnualIncome = i * 10000,
                TotalAssets = i * 20000,
                Address = "Emek",
                City = "Ankara",
                State = "Turkiye",
                PostalCode = "06582",
                DateOfBirth = default,
                EmployerName = "amazon",
                EmploymentPosition = "software developer",
                PhoneNumber = "5632548795",
                DailyTransferAmount = 0,
                Loans = new List<Loan>()
            });
        }

        db.SaveChanges();
    }
}