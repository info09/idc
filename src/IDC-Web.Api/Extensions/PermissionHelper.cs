using IDC.Shared.Constants;

namespace IDC.Api.Extensions;

public static class PermissionHelper
{
    public static string GetPermission(FunctionCode fuctionCode, CommandCode commandCode)
    {
        return $"{fuctionCode}.{commandCode}";
    }
}
