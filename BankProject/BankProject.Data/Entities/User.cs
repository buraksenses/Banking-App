using Microsoft.AspNetCore.Identity;

namespace BankProject.Data.Entities;

public class User : IdentityUser
{
    public float AnnualIncome { get; set; }
    
    public float TotalAssets { get; set; }

    public string Address { get; set; }
    
    public string City { get; set; }
    
    public string State { get; set; }
    
    public string PostalCode { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string EmployerName { get; set; }
    
    public string EmploymentPosition { get; set; }
    
    public string PhoneNumber { get; set; }

    public ICollection<Loan> CreditHistoryDetails { get; set; }
}