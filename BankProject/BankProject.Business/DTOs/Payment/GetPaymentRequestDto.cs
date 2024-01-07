using BankProject.Core.Enums;

namespace BankProject.Business.DTOs.Payment;

public class GetPaymentRequestDto
{
    public decimal Amount { get; set; }
    
    public string TimePeriod { get; set; }

    public int PaymentFrequency { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public string Description { get; set; }
    
    public Guid AccountId { get; set; }
}