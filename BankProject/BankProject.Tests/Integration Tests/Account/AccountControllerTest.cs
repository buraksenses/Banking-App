using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BankProject.API;
using BankProject.Business.DTOs.Account;
using BankProject.Business.DTOs.Auth;
using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Concretes;
using BankProject.Core.Enums;
using BankProject.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankProject.Tests.Integration_Tests.Account;

public class AccountControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AccountControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task CreateUser_Login_CreateAccount_Successfully()
    {
        // Arrange

        // Get database
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var newUser = new CreateUserRequestDto
        {
            Username = "user@email.com",
            Email = "user@email.com",
            Password = "User@1234",
            AnnualIncome = 100000,
            TotalAssets = 1000000,
            Address = "cankaya",
            City = "ankara",
            State = "turkiye",
            PostalCode = "06359",
            DateOfBirth = DateTime.Now.AddYears(-20),
            EmployerName = "amazon",
            EmploymentPosition = "developer",
            PhoneNumber = "5326548958",
            Roles = new List<string>{"Admin"}
        };
        // Act
        
        // Act: Create User
        var userResponse = await _client.PostAsync("api/users", new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json"));
        userResponse.EnsureSuccessStatusCode();
        
        var createdUser = db.Users.OrderByDescending(u => u.CreatedDate).First();
        
        // Act: User Login
        var loginResponse = await _client.PostAsync("api/Auth/login", new StringContent(JsonSerializer.Serialize(
            new LoginRequestDto { Username = newUser.Email, Password = newUser.Password }), Encoding.UTF8, "application/json"));
        loginResponse.EnsureSuccessStatusCode();
        
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(loginContent, options);
        var token = loginResponseDto?.JwtToken;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act: Create Account to User
        var newAccount = new CreateAccountRequestDto
        {
            Balance = 2000,
            AccountType = "Retirement",
            UserId = createdUser.Id
        };
    
        var accountResponse = await _client.PostAsync("api/Accounts", new StringContent(JsonSerializer.Serialize(newAccount), Encoding.UTF8, "application/json"));
        accountResponse.EnsureSuccessStatusCode();
        
        // Get created account from database
        var createdAccount = db.Accounts.OrderByDescending(u => u.CreatedDate).First();

        // Assert
        Assert.NotNull(createdAccount);
        Assert.Equal(createdAccount.Balance, newAccount.Balance);
        Assert.Equal(createdAccount.UserId, createdUser.Id);
    }
    
    [Fact]
    public async Task DepositIntoAccount_UpdatesBalanceAndRecordsTransactionCorrectly()
    {
       // Arrange

        // Get database
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        
        // Act
        var user = await db.Users.SingleAsync(u => u.Id == "1");
        var userAccount = await db.Accounts.SingleAsync(a => a.UserId == user.Id);

        var depositDto = new DepositAndWithdrawDto
        {
            AccountId = userAccount.Id,
            Amount = 200
        };

        // Act: Deposit
        var depositResponse = await _client.PostAsync("api/Transactions/deposit", new StringContent(JsonSerializer.Serialize(depositDto), Encoding.UTF8, "application/json"));
        depositResponse.EnsureSuccessStatusCode();

        var updatedAccount = db.Accounts.AsNoTracking().FirstOrDefault(a => a.Id == userAccount.Id);
        var transactionRecord = db.Transactions.OrderByDescending(t => t.CreatedDate).First();

        // Assert
        Assert.Equal(userAccount.Balance + 200, updatedAccount?.Balance);
        Assert.NotNull(transactionRecord);
        Assert.Equal(depositDto.Amount, transactionRecord.Amount);
        Assert.Equal(TransactionType.Deposit, transactionRecord.TransactionType);
        Assert.Equal(DateTime.UtcNow.Date, transactionRecord.CreatedDate.Date);
    }
    
    
    [Fact]
    public async Task TransferBetweenAccounts_UpdatesBothBalancesCorrectly()
    {
        // Arrange
        // Get database
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        
        var client = _factory.CreateClient();
        var senderUser = await db.Users.SingleAsync(u => u.Id == "1");
        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        var senderBalance = senderAccount.Balance;
        var receiverBalance = receiverAccount.Balance;
        
        const int transferAmount = 200;
        var transferDto = new InternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act: Transfer işlemi yap
        var transferResponse = await client.PostAsync("api/Transactions/transfer/internal", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));
        transferResponse.EnsureSuccessStatusCode();

        var updatedSenderAccount = await db.Accounts.AsNoTracking().SingleAsync(a => a.Id == senderAccount.Id);
        var updatedReceiverAccount = await db.Accounts.AsNoTracking().SingleAsync(a => a.Id == receiverAccount.Id);

        // Assert
        Assert.Equal(senderBalance - 200, updatedSenderAccount.Balance); 
        Assert.Equal(receiverBalance + 200, updatedReceiverAccount.Balance);
    }
    
    [Fact]
    public async Task TransferBetweenAccounts_WithInsufficientFunds_ShouldFail()
    {
        // Arrange
        // Get database
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");

        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        const int transferAmount = 10000; 
        var transferDto = new InternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act: Transfer işlemi yap
        var transferResponse = await _client.PostAsync("api/Transactions/transfer/internal", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, transferResponse.StatusCode); 
    }
    
    [Fact]
    public async Task TransferBetweenAccounts_ExceedingLimitPerInternalTransfer_ShouldFail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");

        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        const decimal transferAmount = AccountService.LimitPerInternalTransfer + 1;
        var transferDto = new InternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act
        var transferResponse = await _client.PostAsync("api/Transactions/transfer/internal", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, transferResponse.StatusCode);
    }
    
    [Fact]
    public async Task TransferBetweenAccounts_ExceedingLimitPerExternalTransfer_ShouldFail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");

        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        const decimal transferAmount = AccountService.LimitPerExternalTransfer + 1; 
        var transferDto = new ExternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act
        var transferResponse = await _client.PostAsync("api/Transactions/transfer/external", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, transferResponse.StatusCode);
    }
    
    [Fact]
    public async Task TransferBetweenAccounts_ExceedingDailyInternalTransferLimit_ShouldFail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");

        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        const decimal transferAmount = AccountService.DailyInternalTransferLimit + 1; 
        var transferDto = new InternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act
        var transferResponse = await _client.PostAsync("api/Transactions/transfer/internal", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, transferResponse.StatusCode);
    }
    
    [Fact]
    public async Task TransferBetweenAccounts_ExceedingDailyExternalTransferLimit_ShouldFail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");
        var receiverUser = await db.Users.SingleAsync(u => u.Id == "2");

        var senderAccount = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
        var receiverAccount = await db.Accounts.SingleAsync(a => a.UserId == receiverUser.Id);

        const decimal transferAmount = AccountService.DailyExternalTransferLimit + 1; 
        var transferDto = new ExternalTransferDto
        {
            SenderId = senderAccount.Id,
            ReceiverId = receiverAccount.Id,
            Amount = transferAmount
        };

        // Act
        var transferResponse = await _client.PostAsync("api/Transactions/transfer/external", new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, transferResponse.StatusCode);
    }
    
    [Fact]
    public async Task Deposit_ExceedingLimitPerDepositAndWithdraw_ShouldFail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();

        var senderUser = await db.Users.SingleAsync(u => u.Id == "3");

        var account = await db.Accounts.SingleAsync(a => a.UserId == senderUser.Id);
       

        var depositAmount = AccountService.LimitPerDepositAndWithdraw + 1;
        var depositDto = new DepositAndWithdrawDto
        {
            AccountId = account.Id,
            Amount = depositAmount
        };

        // Act
        var depositResponse = await _client.PostAsync("api/Transactions/deposit", new StringContent(JsonSerializer.Serialize(depositDto), Encoding.UTF8, "application/json"));

        // Assert
        Assert.NotEqual(HttpStatusCode.OK, depositResponse.StatusCode);
    }
}