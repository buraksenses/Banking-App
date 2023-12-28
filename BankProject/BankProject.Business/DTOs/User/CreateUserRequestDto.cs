﻿namespace BankProject.Business.DTOs.User;

public class CreateUserRequestDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public ICollection<string> Roles { get; set; }
}