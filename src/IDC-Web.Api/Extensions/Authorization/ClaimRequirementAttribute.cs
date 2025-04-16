using IDC.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace IDC.Api.Extensions.Authorization;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(FunctionCode functionCode, CommandCode commandCode) : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { functionCode, commandCode };
    }
}
