using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Concretes;
using BankProject.Core.Enums;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Program = BankProject.API.Program;

namespace BankProject.Tests.Unit_Tests.Account;

public class AccountUnitTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly Mock<IAccountRepository> _mockAccountRepo;
    private readonly Mock<ITransactionRecordRepository> _mockTransactionRepo;
    private readonly Mock<ILoanRepository> _mockLoanRepo;
    private readonly Mock<ITransactionApplicationRepository> _mockTransactionAppRepo;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly AccountService _accountService;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public AccountUnitTests(CustomWebApplicationFactory<Program> factory)
    {
        _mockAccountRepo = new Mock<IAccountRepository>();
        _mockTransactionRepo = new Mock<ITransactionRecordRepository>();
        _mockLoanRepo = new Mock<ILoanRepository>();
        _mockTransactionAppRepo = new Mock<ITransactionApplicationRepository>();
        _factory = factory;

        _mockUserManager = MockUserManager();

        _mockMapper = new Mock<IMapper>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _accountService = new AccountService(
            _mockUserManager.Object,
            _mockMapper.Object,
            _mockUnitOfWork.Object,
            _semaphoreSlim,
            _mockAccountRepo.Object,
            _mockTransactionRepo.Object,
            _mockTransactionAppRepo.Object
        );
    }
    
    private static Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<User>>();
        var userValidators = new List<IUserValidator<User>>();
        var passwordValidators = new List<IPasswordValidator<User>>();
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<User>>>();

        return new Mock<UserManager<User>>(
            store.Object, options.Object, passwordHasher.Object,
            userValidators, passwordValidators, keyNormalizer.Object,
            errors.Object, services.Object, logger.Object
        );
    }
    

    [Fact]
    public async Task Deposit_ShouldUpdateBalanceAndCreateTransactionRecord()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();
    
        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testAccount = db.Accounts.First();
        
        var accountId = testAccount.Id;
        var initialBalance = testAccount.Balance;
        var depositAmount = 200f;
        var expectedBalance = initialBalance + depositAmount;
        SetupAccountRepositoryForTesting(accountId, initialBalance);
    
        // Act
        await _accountService.DepositAsync(accountId, depositAmount);
    
        // Assert
        _mockAccountRepo.Verify(a => a.UpdateBalanceByAccountIdAsync(It.IsAny<Data.Entities.Account>(), expectedBalance), Times.Once);
        _mockTransactionRepo.Verify(t => t.CreateAsync(It.IsAny<TransactionRecord>()), Times.Once);
    }

    [Fact]
    public async Task Withdraw_ShouldUpdateBalanceCorrectly()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 3001) < 1f);
        
        var accountId = testAccount.Id;
        var initialBalance = testAccount.Balance;
        var withdrawAmount = 200f;
        var expectedBalance = initialBalance - withdrawAmount;
        SetupAccountRepositoryForTesting(accountId, initialBalance);

        // Act
        await _accountService.WithdrawAsync(accountId, withdrawAmount);

        // Assert
        _mockAccountRepo.Verify(a => a.UpdateBalanceByAccountIdAsync(It.IsAny<Data.Entities.Account>(), expectedBalance), Times.Once);
        _mockTransactionRepo.Verify(t => t.CreateAsync(It.IsAny<TransactionRecord>()), Times.Once);
    }
    
    [Fact]
    public async Task Transfer_ShouldUpdateBothAccountsCorrectly()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testSenderAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 3001) < 1f);
        var testReceiverAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 6001) < 1f);
        const float transferAmount = 500f;
        var expectedSenderBalance = testSenderAccount.Balance - transferAmount;
        var expectedReceiverBalance = testReceiverAccount.Balance + transferAmount;
        
        var senderId = testSenderAccount.Id;
        var receiverId = testReceiverAccount.Id;
        SetupAccountsForTransfer(senderId, receiverId,
            testSenderAccount.UserId,testReceiverAccount.UserId, 
            testSenderAccount.Balance, testReceiverAccount.Balance);

        // Act
        await _accountService.InternalTransferAsync(senderId, receiverId, transferAmount);

        // Assert
        _mockAccountRepo.Verify(a => 
            a.UpdateAsync(It.Is<Guid>(id => id == senderId), 
                It.Is<Data.Entities.Account>(acc => acc.Id == senderId && Math.Abs(acc.Balance - expectedSenderBalance) < 1f)), Times.Once);
        _mockAccountRepo.Verify(a => 
            a.UpdateAsync(It.Is<Guid>(id => id == receiverId), 
                It.Is<Data.Entities.Account>(acc => acc.Id == receiverId && Math.Abs(acc.Balance - expectedReceiverBalance) < 1f)), Times.Once);
    }
    
    [Fact]
    public async Task CreateAccount_ShouldAddNewAccount()
    {
        // Arrange
        var createAccountDto = new CreateAccountRequestDto
        {
            AccountType = "Deposit",
            Balance = 1000,
            UserId = "3"
        };
        
        var user = new User
        {
            Id = createAccountDto.UserId,
            UserName = "user" + createAccountDto.UserId,
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
            AnnualIncome = int.Parse(createAccountDto.UserId) * 10000,
            TotalAssets = int.Parse(createAccountDto.UserId) * 20000,
            Address = "Emek",
            City = "Ankara",
            State = "Turkiye",
            PostalCode = "06582",
            DateOfBirth = default,
            EmployerName = "amazon",
            EmploymentPosition = "software developer",
            PhoneNumber = "5632548795",
            DailyTransferLimit = 2000,
            DailyTransferAmount = 0,
            Loans = new List<Loan>()
        };
        
        _mockMapper.Setup(mapper => mapper.Map<Data.Entities.Account>(createAccountDto))
            .Returns(new Data.Entities.Account
            {
                UserId = createAccountDto.UserId,
                Balance = createAccountDto.Balance,
                AccountType = AccountType.Deposit
            });
        
        _mockUserManager.Setup(manager => manager.FindByIdAsync(createAccountDto.UserId))
            .ReturnsAsync(user);

        // Act
        await _accountService.CreateAccountAsync(createAccountDto);

        // Assert
        _mockAccountRepo.Verify(a => a.CreateAsync(It.IsAny<Data.Entities.Account>()), Times.Once);
    }
    
    [Fact]
    public async Task WithdrawAsync_InsufficientBalance_ThrowsException()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 3001) < 1f);
        const float withdrawAmount = 6000f;
        
        var account = new Data.Entities.Account { Id = testAccount.Id, Balance = testAccount.Balance };
    
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(testAccount.Id))
            .ReturnsAsync(account);
    
        await Assert.ThrowsAsync<InvalidOperationException>(() => _accountService.WithdrawAsync(testAccount.Id, withdrawAmount));
    }
    
    [Fact]
    public async Task GetBalanceByAccountIdAsync_ValidId_ReturnsCorrectBalance()
    {
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 3001) < 1f);

        var account = new Data.Entities.Account { Id = testAccount.Id, Balance = testAccount.Balance };
    
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(testAccount.Id))
            .ReturnsAsync(account);
    
        var balance = await _accountService.GetBalanceByAccountIdAsync(testAccount.Id);
    
        Assert.Equal(testAccount.Balance, balance);
    }
    
    [Fact]
    public async Task UpdateBalanceByAccountIdAsync_NegativeBalance_ThrowsException()
    {
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 3001) < 1f);
        
        var account = new Data.Entities.Account { Id = testAccount.Id, Balance = testAccount.Balance };
    
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(testAccount.Id))
            .ReturnsAsync(account);
    
        await Assert.ThrowsAsync<InvalidOperationException>(() => _accountService.UpdateBalanceByAccountIdAsync(testAccount.Id, -100f));
    }
    
    [Fact]
    public async Task InternalTransfer_ExceedsLimit_ThrowsInvalidOperationException()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BankDbContext>();
        var testSenderAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 15001) < 1f);
        var testReceiverAccount = await db.Accounts.SingleAsync(account => Math.Abs(account.Balance - 6001) < 1f);
        const float transferAmount = 15000f;

        var senderId = testSenderAccount.Id;
        var receiverId = testReceiverAccount.Id;

        SetupAccountsForTransfer(senderId, receiverId, 
            testSenderAccount.UserId, testReceiverAccount.UserId, 
            testSenderAccount.Balance, testReceiverAccount.Balance);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _accountService.InternalTransferAsync(senderId, receiverId, transferAmount));
    }
    
    private void SetupAccountRepositoryForTesting(Guid accountId, float initialBalance)
    {
        var account = new Data.Entities.Account
        {
            Id = accountId,
            Balance = initialBalance
        };
        
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId))
            .ReturnsAsync(account);
    }
    
    private void SetupAccountsForTransfer(Guid senderId, Guid receiverId, string senderUserId, string receiverUserId, float senderBalance, float receiverBalance)
    {
        var senderAccount = new Data.Entities.Account
        {
            Id = senderId,
            Balance = senderBalance, 
            UserId = senderUserId
        };

        var receiverAccount = new Data.Entities.Account
        {
            Id = receiverId,
            Balance = receiverBalance,
            UserId = receiverUserId
        };

        var senderUser = new User
        {
            Id = senderUserId,
            UserName = "user" + senderUserId,
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
            AnnualIncome = int.Parse(senderUserId) * 10000,
            TotalAssets = int.Parse(senderUserId) * 20000,
            Address = "Emek",
            City = "Ankara",
            State = "Turkiye",
            PostalCode = "06582",
            DateOfBirth = default,
            EmployerName = "amazon",
            EmploymentPosition = "software developer",
            PhoneNumber = "5632548795",
            DailyTransferLimit = 2000,
            DailyTransferAmount = 0,
            Loans = new List<Loan>()
        };
        
        var receiverUser = new User
        {
            Id = receiverUserId,
            UserName = "user" + receiverUserId,
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
            AnnualIncome = int.Parse(receiverUserId) * 10000,
            TotalAssets = int.Parse(receiverUserId) * 20000,
            Address = "Emek",
            City = "Ankara",
            State = "Turkiye",
            PostalCode = "06582",
            DateOfBirth = default,
            EmployerName = "amazon",
            EmploymentPosition = "software developer",
            PhoneNumber = "5632548795",
            DailyTransferLimit = 2000,
            DailyTransferAmount = 0,
            Loans = new List<Loan>()
        };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(senderId))
            .ReturnsAsync(senderAccount);
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(receiverId))
            .ReturnsAsync(receiverAccount);
        _mockUserManager.Setup(manager => manager.FindByIdAsync(senderUserId))
            .ReturnsAsync(senderUser);
        _mockUserManager.Setup(manager => manager.FindByIdAsync(receiverUserId))
            .ReturnsAsync(receiverUser);
        _mockAccountRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Data.Entities.Account>()))
            .Returns(Task.CompletedTask);
        _mockTransactionRepo.Setup(repo => repo.CreateAsync(It.IsAny<TransactionRecord>()))
            .Returns(Task.CompletedTask);
        _mockUserManager.Setup(manager => manager.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
    }

}

