using Microsoft.AspNetCore.Authorization;
using OSD_HR_Management_Backend.Constants;
using OSD_HR_Management_Backend.Logics.Abstractions;
using OSD_HR_Management_Backend.Repositories.Abstractions;

namespace OSD_HR_Management_Backend.Middlewares;

public class AdminPermission : IAuthorizationRequirement
{
    public AdminPermission() { }
}

public class AdminPermissionHandler : AuthorizationHandler<AdminPermission>
{
    private readonly IUserLogic _userLogic;
    public AdminPermissionHandler(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminPermission requirement)
    {
        if(!context.User.HasClaim(x => x.Type == "UserId"))
        {
            return Task.CompletedTask;
        }

        var id = context.User.Claims.Where(x => x.Type == "UserId")
                                .Select(x => x.Value).SingleOrDefault();      

        if(id != null)
        {
            var existingUser = _userLogic.GetUserById(id);
            if(existingUser != null && existingUser.Result.Role == Roles.Admin)
            {
                context.Succeed(requirement);
            }            
        }
        return Task.CompletedTask;
    }
}
