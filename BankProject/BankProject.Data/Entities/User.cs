namespace BankProject.Data.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    //Navigation Properties
    public ICollection<Account> Accounts { get; set; }
}