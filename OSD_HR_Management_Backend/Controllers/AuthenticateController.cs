using Amazon.Runtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OSD_HR_Management_Backend.Constants;
using OSD_HR_Management_Backend.Logics.Abstractions;
using OSD_HR_Management_Backend.Logics.Helpers;
using OSD_HR_Management_Backend.Repositories.Models;
using OSD_HR_Management_Backend.RequestModels;
using OSD_HR_Management_Backend.Services.Abstractions;
using OSD_HR_Management_Backend.Services.Implementations;
using OSD_HR_Management_Backend.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;

namespace OSD_HR_Management_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserLogic _userLogic;
    private readonly IStorageService _storeService;

    public AuthenticateController(IConfiguration configuration, IUserLogic userLogic, IStorageService storeService)
    {
        _configuration = configuration;
        _userLogic = userLogic;
        _storeService = storeService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
    {
        var decodedPassword = EncodingHelper.EncodePasswordToBase64(requestModel.Password);
        var allUsers = await _userLogic.GetAllUsers();

        var existingUser = allUsers.FirstOrDefault(u =>
            u.Username == requestModel.Username &&
            u.Password == decodedPassword);

        if (existingUser == null)
        {
            throw new Exception("User doesn't exist!");
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, requestModel.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("UserId", existingUser.UserId)
        };

        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    [Authorize(Policy = Policies.AdminOnly)]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        var avatarPath = string.Empty;
        if (requestModel.Avatar != null)
        {
            var objUpload = new S3ObjectUpload(requestModel.Avatar, "osd-hr-management", "public/image");
            avatarPath = await _storeService.UploadFileAsync(objUpload);
        }

        var registered = await _userLogic.SaveUser(requestModel, avatarPath);
        return Ok(registered);
    }

    [Authorize]
    [HttpGet]
    [Route("users")]
    public async Task<IActionResult> GetUsersPortal()
    {
        var result = await _userLogic.GetUsersPortal();
        Console.WriteLine(result);

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    [Route("decode-token")]
    public async Task<IActionResult> DecodeToken([FromHeader] string jwtToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);

        var userId = jwt.Claims.First(c => c.Type == "UserId").Value;

        var user = await _userLogic.GetUserById(userId);

        return Ok(user);
    }
}
