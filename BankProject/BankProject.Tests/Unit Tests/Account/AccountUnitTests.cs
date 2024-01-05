using AutoMapper;
using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Concretes;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BankProject.Tests.Unit_Tests.Account;

public class AccountUnitTests
{
     private readonly Mock<IAccountRepository> _mockAccountRepo;
    private readonly Mock<ITransactionRecordRepository> _mockTransactionRepo;
    private readonly Mock<ILoanRepository> _mockLoanRepo;
    private readonly Mock<ITransactionApplicationRepository> _mockTransactionAppRepo;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly AccountService _accountService;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public AccountUnitTests()
    {
        _mockAccountRepo = new Mock<IAccountRepository>();
        _mockTransactionRepo = new Mock<ITransactionRecordRepository>();
        _mockLoanRepo = new Mock<ILoanRepository>();
        _mockTransactionAppRepo = new Mock<ITransactionApplicationRepository>();

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
    public async Task DepositAsync_UpdatesBalanceCorrectly()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var amount = 100.0f;
        var initialBalance = 200.0f;
        var expectedBalance = initialBalance + amount;
        var account = new Data.Entities.Account { Id = accountId, Balance = initialBalance };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockAccountRepo.Setup(repo => repo.UpdateBalanceByAccountIdAsync(It.IsAny<Data.Entities.Account>(), It.IsAny<float>()))
            .Callback<Data.Entities.Account, float>((acc, amt) => acc.Balance = amt);

        // Act
        await _accountService.DepositAsync(accountId, amount);

        // Assert
        Assert.Equal(expectedBalance, account.Balance);
    }
    
    [Fact]
    public async Task CreateAccountAsync_CreatesAccountSuccessfully()
    {
        // Arrange
        var createAccountRequestDto = new CreateAccountRequestDto
        {
            AccountType = "Deposit",
            Balance = 1000,
            UserId = "1"
        };
        var expectedAccount = new Data.Entities.Account
        {
            Id = Guid.NewGuid(),
            AccountType = AccountType.Deposit,
            Balance = createAccountRequestDto.Balance,
            CreatedDate = DateTime.UtcNow,
            UserId = createAccountRequestDto.UserId
        };
        
        _mockMapper.Setup(m => m.Map<Data.Entities.Account>(It.IsAny<CreateAccountRequestDto>())).Returns(expectedAccount);
        _mockAccountRepo.Setup(repo => repo.CreateAsync(It.IsAny<Data.Entities.Account>()));
        _mockUserManager.Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => new User { Id = id, UserName = "TestUser"});

        // Act
        await _accountService.CreateAccountAsync(createAccountRequestDto);

        // Assert
        _mockAccountRepo.Verify(repo => repo.CreateAsync(expectedAccount), Times.Once);
    }
    
    [Fact]
    public async Task GetBalanceByAccountIdAsync_ReturnsCorrectBalance_WhenAccountExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var expectedBalance = 1000.0f;
        var account = new Data.Entities.Account { Id = accountId, Balance = expectedBalance };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Act
        var balance = await _accountService.GetBalanceByAccountIdAsync(accountId);

        // Assert
        Assert.Equal(expectedBalance, balance);
    }
    
    [Fact]
    public async Task GetBalanceByAccountIdAsync_ThrowsNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync((Data.Entities.Account)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _accountService.GetBalanceByAccountIdAsync(accountId));
    }
    
    [Fact]
    public async Task CreateAccountAsync_CreatesAccountWithCorrectInformation()
    {
        // Arrange
        var createAccountRequestDto = new CreateAccountRequestDto
        {
            AccountType = "Deposit",
            Balance = 1000,
            UserId = "1"
        };
        var account = new Data.Entities.Account
        {
            AccountType = AccountType.Deposit,
            Balance = createAccountRequestDto.Balance,
            CreatedDate = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            UserId = createAccountRequestDto.UserId
        };

        _mockMapper.Setup(m => m.Map<Data.Entities.Account>(createAccountRequestDto)).Returns(account);
        _mockUserManager.Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => new User { Id = id, UserName = "TestUser"});
        _mockAccountRepo.Setup(repo => repo.CreateAsync(account)).Verifiable();

        // Act
        await _accountService.CreateAccountAsync(createAccountRequestDto);

        // Assert
        _mockAccountRepo.Verify();
    }
    
    [Fact]
    public async Task CreateAccountAsync_ThrowsArgumentException_WhenAccountTypeIsInvalid()
    {
        // Arrange
        var createAccountRequestDto = new CreateAccountRequestDto { AccountType = "InvalidType" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _accountService.CreateAccountAsync(createAccountRequestDto));
    }
    
    [Fact]
    public async Task CreateAccountAsync_ThrowsNullReferenceException_WhenUserDoesNotExist()
    {
        // Arrange
        var createAccountRequestDto = new CreateAccountRequestDto { UserId = "5", AccountType = "Deposit", Balance = 1000};
        _mockUserManager.Setup(manager => manager.FindByIdAsync(createAccountRequestDto.UserId)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _accountService.CreateAccountAsync(createAccountRequestDto));
    }
    
    [Fact]
    public async Task UpdateBalanceByAccountIdAsync_UpdatesBalanceCorrectly_WhenAccountExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float newBalance = 1000.0f;
        var account = new Data.Entities.Account { Id = accountId, Balance = 500.0f };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockAccountRepo.Setup(repo => repo.UpdateBalanceByAccountIdAsync(account, newBalance))
            .Callback<Data.Entities.Account, float>((acc, bal) => acc.Balance = bal);

        // Act
        await _accountService.UpdateBalanceByAccountIdAsync(accountId, newBalance);

        // Assert
        Assert.Equal(newBalance, account.Balance);
    }
    
    [Fact]
    public async Task UpdateBalanceByAccountIdAsync_ThrowsInvalidOperationException_WhenBalanceIsNegative()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float negativeBalance = -100.0f;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _accountService.UpdateBalanceByAccountIdAsync(accountId, negativeBalance));
    }
    
    [Fact]
    public async Task UpdateBalanceByAccountIdAsync_ThrowsNotFoundException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float newBalance = 1000.0f;
        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync((Data.Entities.Account)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await _accountService.UpdateBalanceByAccountIdAsync(accountId, newBalance));
    }
    
    
    [Fact]
    public async Task DepositAsync_UpdatesBalanceCorrectly_WhenDepositIsSuccessful()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float depositAmount = 500.0f;
        const float initialBalance = 1000.0f;
        var account = new Data.Entities.Account { Id = accountId, Balance = initialBalance };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockAccountRepo.Setup(repo => repo.UpdateBalanceByAccountIdAsync(account, initialBalance + depositAmount))
            .Callback<Data.Entities.Account, float>((acc, bal) => acc.Balance = bal);

        // Act
        await _accountService.DepositAsync(accountId, depositAmount);

        // Assert
        Assert.Equal(initialBalance + depositAmount, account.Balance);
    }
    
    [Fact]
    public async Task WithdrawAsync_UpdatesBalanceCorrectly_WhenWithdrawalIsSuccessful()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float withdrawalAmount = 500.0f;
        const float initialBalance = 1000.0f;
        var account = new Data.Entities.Account { Id = accountId, Balance = initialBalance };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockAccountRepo.Setup(repo => repo.UpdateBalanceByAccountIdAsync(account, initialBalance - withdrawalAmount))
            .Callback<Data.Entities.Account, float>((acc, bal) => acc.Balance = bal);

        // Act
        await _accountService.WithdrawAsync(accountId, withdrawalAmount);

        // Assert
        Assert.Equal(initialBalance - withdrawalAmount, account.Balance);
    }
    
    [Fact]
    public async Task DepositAsync_ThrowsInvalidOperationException_WhenAmountExceedsDailyLimit()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const decimal amountExceedingLimit = AccountService.LimitPerDepositAndWithdraw + 1;
        var account = new Data.Entities.Account { Id = accountId, Balance = 1000.0f, AccountType = AccountType.Deposit};

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);


        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _accountService.DepositAsync(accountId, (float)amountExceedingLimit));
    }
    
    [Fact]
    public async Task WithdrawAsync_ThrowsInvalidOperationException_WhenAmountExceedsDailyLimit()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const decimal amountExceedingLimit = AccountService.LimitPerDepositAndWithdraw + 1;
        var account = new Data.Entities.Account { Id = accountId, Balance = 10000.0f };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _accountService.WithdrawAsync(accountId, (float)amountExceedingLimit));
    }
    
    [Fact]
    public async Task WithdrawAsync_ThrowsInvalidOperationException_WhenBalanceIsInsufficient()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const float withdrawalAmount = 1500.0f; // Withdrawal amount is more than balance
        const float initialBalance = 1000.0f;
        var account = new Data.Entities.Account { Id = accountId, Balance = initialBalance };

        _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _accountService.WithdrawAsync(accountId, withdrawalAmount));
    }

}