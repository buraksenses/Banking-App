﻿namespace BankProject.Data.Entities;

public class Account
{
    public Guid Id { get; set; }

    public float Balance { get; set; }

    public string AccountType { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UserId { get; set; }
}