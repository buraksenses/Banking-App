﻿using System.Text;
using BankProject.Business.Mappings;
using BankProject.Business.Services.Concretes;
using BankProject.Business.Services.Interfaces;
using BankProject.Business.Validators.Account;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BankProject.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",new OpenApiInfo{Title = "Bank API", Version = "v1"});
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "Oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        services.AddHangfire((sp, globalConfiguration) =>
        {
            globalConfiguration.UseSqlServerStorage(sp.GetRequiredService<IConfiguration>()
                .GetConnectionString("BankConnectionString"));
        });
        services.AddHangfireServer();

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("BankConnectionString")));

        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddSingleton(new SemaphoreSlim(1, 1));
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<ITransactionRecordRepository, TransactionRecordRepository>();

        services.AddScoped<ITokenRepository, TokenRepository>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ILoanApplicationService, LoanApplicationService>();

        services.AddScoped<ICreditScoreService, CreditScoreService>();

        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<ILoanRepository, LoanRepository>();

        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUnitOfWork, UnitOfWork>(serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<BankDbContext>();
            var isInMemory = dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
            return new UnitOfWork(dbContext, isInMemory);
        });

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();

        services.AddScoped<ITransactionApplicationRepository, TransactionApplicationRepository>();
        services.AddScoped<ITransactionApplicationService, TransactionApplicationService>();

        services.AddFluentValidation(fv => 
            fv.RegisterValidatorsFromAssemblyContaining<CreateAccountValidator>());
        
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>("Bank")
            .AddEntityFrameworkStores<BankDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        });
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
            });
        
        return services;
    }
}