namespace BankProject.Business.DTOs.Account;

public class CreateAccountRequestDto
{
    public float Balance { get; set; }
    public string AccountType { get; set; }
    
    public Guid UserId { get; set; }
}