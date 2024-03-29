using BankProject.API.Extensions;
using BankProject.API.Middlewares;
using BankProject.Business.Services.Interfaces;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    recurringJobManager.AddOrUpdate(
        "ResetDailyTransferLimits",
        () => userService.ResetDailyLimits(),
        "*/3 * * * *"
    );
}

app.Run();