using Amazon.DynamoDBv2.DataModel;

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
    public Boolean IsActive { get; set; } = true;

    [DynamoDBProperty("createAt")]
    public DateTime CreateAt { get; set; }

/*    [DynamoDBProperty("jobTitle")]
    public string JobTitle { get; set; }

    [DynamoDBProperty("email")]
    public string Email { get; set; }

    [DynamoDBProperty("fullName")]
    public string FullName { get; set; }

    [DynamoDBProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = null!;

    [DynamoDBProperty("skype")]
    public string Skype { get; set; } = null!;

    [DynamoDBProperty("salary")]
    public ulong Salary { get; set; } = 0;*/
}
