using BankProject.Business.Mappings;
using BankProject.Business.Services.Concretes;
using BankProject.Business.Services.Interfaces;
using BankProject.Data.Context;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
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
        
        return services;
    }
}