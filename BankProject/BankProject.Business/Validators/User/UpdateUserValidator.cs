using BankProject.Business.DTOs.User;
using FluentValidation;

namespace BankProject.Business.Validators.User;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Roles)
            .NotNull().WithMessage("Roles must not be null.")
            .Must(r => r != null && r.All(IsValidRole))
            .WithMessage("Invalid role.");
    }

    private static bool IsValidRole(string role)
    {
        var validRoles = new[] { "Admin", "Customer", "Auditor" };
        return validRoles.Contains(role);
    }
}