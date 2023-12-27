namespace BankProject.Business.DTOs.Account;

public class CreateAccountRequestDto
{
    public float Balance { get; set; }
    public string AccountType { get; set; }
    public string UserId { get; set; }
}