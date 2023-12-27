using BankProject.Business.Mappings;
using BankProject.Business.Security;
using BankProject.Business.Security.Concrete;
using BankProject.Business.Security.Interface;
using BankProject.Business.Services.Concretes;
using BankProject.Business.Services.Interfaces;
using BankProject.Business.Validators.Account;
using BankProject.Data.Context;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace BankProject.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("BankConnectionString")));

        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddFluentValidation(fv => 
            fv.RegisterValidatorsFromAssemblyContaining<CreateAccountValidator>());
        
        return services;
    }
}