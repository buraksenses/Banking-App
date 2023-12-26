namespace BankProject.Data.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public Guid RoleId { get; set; }
    
    //Navigation Properties
    public Role Role { get; set; }

    public ICollection<Account> Accounts { get; set; }
}