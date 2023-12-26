using BankProject.Business.DTOs.Account;
using FluentValidation;

namespace BankProject.Business.Validators.Account;

public class InternalTransferValidator : AbstractValidator<InternalTransferDto>
{
    public InternalTransferValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("Sender ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Account ID.");
        
        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("Receiver ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Account ID.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.")
            .LessThan(10000).WithMessage("You can send a maximum of $50000 at a time");
    }
    
    private static bool BeAValidGuid(Guid accountId)
    {
        return accountId != Guid.Empty && accountId.ToString().Length == 36;
    }
}