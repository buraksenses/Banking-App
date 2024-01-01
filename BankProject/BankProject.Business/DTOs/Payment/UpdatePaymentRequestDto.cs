namespace BankProject.Business.DTOs.Payment;

public class UpdatePaymentRequestDto
{
    public float Amount { get; set; }
    
    public string TimePeriod { get; set; }

    public int PaymentFrequency { get; set; }

    public string Description { get; set; }
}