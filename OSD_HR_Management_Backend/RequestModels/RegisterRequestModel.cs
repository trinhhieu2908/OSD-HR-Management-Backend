﻿using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OSD_HR_Management_Backend.Repositories.Models;

public class RegisterRequestModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; } = "Employee";

    public IFormFile Avatar { get; set; } = null;

    [Required]
    public string JobTitle { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string FullName { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Skype { get; set; } = string.Empty;
}
