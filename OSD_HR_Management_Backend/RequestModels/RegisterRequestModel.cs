using System.ComponentModel.DataAnnotations;

namespace OSD_HR_Management_Backend.Repositories.Models;

public class RegisterRequestModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string? Role { get; set; } = "Employee";

    public IFormFile? Avatar { get; set; }

    /*[Required]
    public string JobTitle { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string FullName { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Skype { get; set; } = null!;

    public ulong Salary { get; set; } = 0;*/
}
