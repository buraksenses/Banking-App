using BankProject.Business.DTOs.Loan;
using FluentValidation;

namespace BankProject.Business.Validators.Loan;

public class CreateLoanValidator : AbstractValidator<CreateLoanRequestDto>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.LoanType)
            .NotEmpty().WithMessage("Loan type is required.")
            .Must(BeAValidLoanType).WithMessage("Invalid loan type.");

        RuleFor(x => x.LoanAmount)
            .GreaterThan(0).WithMessage("Loan amount must be greater than zero.");

        RuleFor(x => x.LoanTerm)
            .GreaterThan(0).WithMessage("Loan term must be a positive number.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
    
    private static bool BeAValidLoanType(string loanType)
    {
        var validLoanTypes = new[] { "Personal", "Mortgage", "Vehicle", "Education", "SmallBusiness" };
        return validLoanTypes.Contains(loanType);
    }
}