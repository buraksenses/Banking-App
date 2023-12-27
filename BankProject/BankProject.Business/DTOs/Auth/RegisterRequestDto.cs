﻿using System.ComponentModel.DataAnnotations;

namespace BankProject.Business.DTOs.Auth;

public class RegisterRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string[] Roles { get; set; }
}