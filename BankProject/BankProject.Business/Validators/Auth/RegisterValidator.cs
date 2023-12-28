using BankProject.Business.DTOs.Auth;
using FluentValidation;

namespace BankProject.Business.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterRequestDto>
{
    private readonly List<string> _validRoles = new()
    {
        "Admin", "Customer", "Bank_Officer", "Advisor", "Loan_Officer", "Auditor"
    };

    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .EmailAddress().WithMessage("Username must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        When(x => x.Roles.Any(), () =>
        {
            RuleForEach(x => x.Roles)
                .Must(role => _validRoles.Contains(role))
                .WithMessage("Invalid role.");
        });
    }
}