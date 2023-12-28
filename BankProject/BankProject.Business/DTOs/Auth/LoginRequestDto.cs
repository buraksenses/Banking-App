using System.ComponentModel.DataAnnotations;

namespace BankProject.Business.DTOs.Auth;

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}