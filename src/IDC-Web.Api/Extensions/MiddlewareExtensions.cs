using IDC.Api.Middlewares;

namespace IDC.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorWrapping(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorWrappingMiddleware>();
    }
}
