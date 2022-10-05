﻿namespace OSD_HR_Management_Backend.ResponseModels;

#nullable disable

public class GetUserResponseModel
{
    public string UserId { get; set; }

    public string FullName { get; set; }

    public string Avatar { get; set; }

    public string Role { get; set; }

    public Boolean IsActive { get; set; }

    public string JobTitle { get; set; }

    public string Email { get; set; }    

    public string PhoneNumber { get; set; }

    public string Skype { get; set; }
}
