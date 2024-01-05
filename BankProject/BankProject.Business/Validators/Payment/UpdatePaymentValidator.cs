using BankProject.Business.DTOs.Payment;
using FluentValidation;

namespace BankProject.Business.Validators.Payment;

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentRequestDto>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("Time period is required.")
            .Must(BeAValidTimePeriod).WithMessage("Invalid time period.");

        RuleFor(x => x.PaymentFrequency)
            .GreaterThan(0).WithMessage("Payment frequency must be a positive number.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
    
    private static bool BeAValidTimePeriod(string timePeriod)
    {
        var validTimePeriods = new[] { "Months", "Minutes", "Years", "Days" };
        return validTimePeriods.Contains(timePeriod);
    }
}