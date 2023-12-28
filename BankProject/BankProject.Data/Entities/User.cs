using Microsoft.AspNetCore.Identity;

namespace BankProject.Data.Entities;

public class User : IdentityUser
{
    public decimal CreditScore { get; set; }
    
    public decimal AnnualIncome { get; set; }
    
    public decimal TotalAssets { get; set; }
    
    public string CreditHistory { get; set; }
    
    public string Address { get; set; }
    
    public string City { get; set; }
    
    public string State { get; set; }
    
    public string PostalCode { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string EmployerName { get; set; }
    
    public string EmploymentPosition { get; set; }
    
    public string PhoneNumber { get; set; }
}