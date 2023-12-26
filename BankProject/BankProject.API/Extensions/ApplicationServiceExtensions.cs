using BankProject.Data.Context;
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
        
        return services;
    }
}