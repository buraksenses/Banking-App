namespace BankProject.Business.DTOs.User;

public class CreateUserRequestDto
{
    public string Username { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }

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
    public ICollection<string> Roles { get; set; }
}