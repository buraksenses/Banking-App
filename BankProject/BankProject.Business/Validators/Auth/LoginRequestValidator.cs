namespace BankProject.Business.Validators.Auth;

using BankProject.Business.DTOs.Auth;
using FluentValidation;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .EmailAddress().WithMessage("Username must be a valid email address.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}