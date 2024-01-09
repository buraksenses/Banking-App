using BankProject.Business.DTOs.Account;
using FluentValidation;

namespace BankProject.Business.Validators.Account;

public class DepositAndWithdrawValidator : AbstractValidator<DepositAndWithdrawDto>
{
    public DepositAndWithdrawValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }

    private static bool BeAValidGuid(Guid accountId)
    {
        return accountId != Guid.Empty && accountId.ToString().Length == 36;
    }
    
}