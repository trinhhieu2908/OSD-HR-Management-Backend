using Amazon.DynamoDBv2.DataModel;

#nullable disable

namespace OSD_HR_Management_Backend.Repositories.Models;

[DynamoDBTable("users")]
public class UserModel
{
    [DynamoDBHashKey("userId")]
    public string UserId { get; set; }

    [DynamoDBProperty("username")]
    public string Username { get; set; }

    [DynamoDBProperty("password")]
    public string Password { get; set; }

    [DynamoDBProperty("avatar")]
    public string Avatar { get; set; }

    [DynamoDBProperty("role")]
    public string Role { get; set; } = "Employee";

    [DynamoDBProperty("isActive")]
    public Boolean IsActive { get; set; }    

    [DynamoDBProperty("jobTitle")]
    public string JobTitle { get; set; }

    [DynamoDBProperty("email")]
    public string Email { get; set; }

    [DynamoDBProperty("fullName")]
    public string FullName { get; set; }

    [DynamoDBProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [DynamoDBProperty("skype")]
    public string Skype { get; set; } = string.Empty;

    [DynamoDBProperty("createAt")]
    public DateTime CreateAt { get; set; }

    [DynamoDBProperty("updateAt")]
    public DateTime UpdateAt { get; set; }
}
