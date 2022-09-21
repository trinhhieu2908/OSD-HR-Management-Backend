using Microsoft.AspNetCore.Authorization;
using OSD_HR_Management_Backend.Constants;

namespace OSD_HR_Management_Backend.Middlewares;

public class AdminPermission : IAuthorizationRequirement
{
    public AdminPermission() { }
}

public class AdminPermissionHandler : AuthorizationHandler<AdminPermission>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminPermission requirement)
    {
        if(!context.User.HasClaim(x => x.Type == "Role"))
        {
            return Task.CompletedTask;
        }

        var role = context.User.Claims.Where(x => x.Type == "Role" && x.Value == Roles.Employee);
        if (role.Any())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
