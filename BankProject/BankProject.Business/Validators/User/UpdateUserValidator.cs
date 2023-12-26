using BankProject.API.DTOs.User;
using FluentValidation;

namespace BankProject.Business.Validators.User;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
    
    private static bool BeAValidGuid(Guid accountId)
    {
        return accountId != Guid.Empty && accountId.ToString().Length == 36;
    }
}