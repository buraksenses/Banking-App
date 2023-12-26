namespace BankProject.Business.DTOs.User;

public class CreateUserRequestDto
{
    public string Name { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public Guid RoleId { get; set; }
}