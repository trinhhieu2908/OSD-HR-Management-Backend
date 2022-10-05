using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OSD_HR_Management_Backend.RequestModels;

public class LoginRequestModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
