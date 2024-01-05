using BankProject.Business.DTOs.SupportTicket;
using FluentValidation;

namespace BankProject.Business.Validators.SupportTicket;

public class CreateSupportTicketValidator : AbstractValidator<CreateSupportTicketRequestDto>
{
    public CreateSupportTicketValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters."); // Length constraints can be adjusted

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(10, 1000).WithMessage("Description must be between 10 and 1000 characters."); // Length constraints can be adjusted

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}