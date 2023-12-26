﻿using BankProject.Core.Enums;

namespace BankProject.Data.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public float Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid AccountId { get; set; }

    public Guid? ReceiverAccountId { get; set; }
    
    //Navigation Properties
    public Account Account { get; set; }

    public Account ReceiverAccount { get; set; }
}