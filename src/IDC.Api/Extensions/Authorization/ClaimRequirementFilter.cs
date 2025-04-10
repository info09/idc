using IDC.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace IDC.Api.Extensions.Authorization;

public class ClaimRequirementFilter : IAuthorizationFilter
{
    private readonly CommandCode _commandCode;
    private readonly FunctionCode _functionCode;

    public ClaimRequirementFilter(CommandCode commandCode, FunctionCode functionCode)
    {
        _commandCode = commandCode;
        _functionCode = functionCode;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permissionClaims = context.HttpContext.User.Claims.SingleOrDefault(i => i.Type.Equals(SystemConsts.Claims.Permissions));
        if (permissionClaims == null)
        {
            context.Result = new ForbidResult();
        }
        else
        {
            var permissions = JsonSerializer.Deserialize<List<string>>(permissionClaims.Value);
            if (!permissions!.Contains(PermissionHelper.GetPermission(_functionCode, _commandCode)))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
