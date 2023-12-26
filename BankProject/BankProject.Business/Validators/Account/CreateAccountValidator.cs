using BankProject.Business.DTOs.Account;
using FluentValidation;

namespace BankProject.Business.Validators.Account;

public class CreateAccountValidator : AbstractValidator<CreateAccountRequestDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("Balance cannot be negative.");

        
        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required.")
            .Must(BeAValidType).WithMessage("Invalid account type.");

        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid or incorrect length User ID.");
    }
    
    private static bool BeAValidType(string accountType)
    {
        
        var validTypes = new[] { "Checking", "Savings", "Deposit", "Investment", "Retirement", "Joint", "Student" };
        return validTypes.Contains(accountType);
    }

    private static bool BeAValidGuid(Guid userId)
    {
        return userId != Guid.Empty && userId.ToString().Length == 36;
    }
}