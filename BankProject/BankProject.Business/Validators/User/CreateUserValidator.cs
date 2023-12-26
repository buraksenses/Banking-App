using BankProject.Business.DTOs.User;
using FluentValidation;

namespace BankProject.Business.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Role ID.");
    }
    
    private static bool BeAValidGuid(Guid accountId)
    {
        return accountId != Guid.Empty && accountId.ToString().Length == 36;
    }
}