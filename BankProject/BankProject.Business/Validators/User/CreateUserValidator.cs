using System.Text.RegularExpressions;
using BankProject.Business.DTOs.User;
using FluentValidation;

namespace BankProject.Business.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50); // Example length constraints

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.AnnualIncome)
            .GreaterThanOrEqualTo(0).WithMessage("Annual income must be a positive number.");

        RuleFor(x => x.TotalAssets)
            .GreaterThanOrEqualTo(0).WithMessage("Total assets must be a positive number.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .Length(5, 200); 

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .Length(2, 100); 

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .Length(2, 100); 

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .Matches(new Regex("^[0-9]{5}(-[0-9]{4})?$"))
            .WithMessage("Invalid postal code format.");

        RuleFor(x => x.EmployerName)
            .NotEmpty().WithMessage("Employer name is required.")
            .Length(2, 100); 

        RuleFor(x => x.EmploymentPosition)
            .NotEmpty().WithMessage("Employment position is required.")
            .Length(2, 100); 

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(new Regex("^[0-9]{10}$")).WithMessage("Invalid phone number format.");

        RuleFor(x => x.Roles)
            .NotNull().WithMessage("Roles must not be null.")
            .Must(r => r.All(IsValidRole)).WithMessage("Invalid role."); 
    }

    private static bool IsValidRole(string role)
    {
        var validRoles = new[] { "Admin", "Customer", "Auditor" };
        return validRoles.Contains(role);
    }
}