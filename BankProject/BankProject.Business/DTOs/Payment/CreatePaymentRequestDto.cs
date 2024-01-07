﻿namespace BankProject.Business.DTOs.Payment;

public class CreatePaymentRequestDto
{
    public decimal Amount { get; set; }
    
    public string TimePeriod { get; set; }

    public int PaymentFrequency { get; set; }

    public string Description { get; set; }
    
    public Guid AccountId { get; set; }
}