using IDC.Shared.SeedWorks;
using System.Text.Json;

namespace IDC.Api.Middlewares;

public class ErrorWrappingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorWrappingMiddleware> _logger;

    public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        string errorMsg = string.Empty;
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            errorMsg = ex.Message;
            context.Response.StatusCode = 500;
            var resp = new ApiErrorResult<bool>(500, errorMsg);
            var json = JsonSerializer.Serialize(resp);
            await context.Response.WriteAsync(json);
        }

        if (!context.Response.HasStarted && context.Response.StatusCode == 204)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; // Override 204 to 200 with body
            var resp = new ApiErrorResult<bool>(204, "No content");
            var json = JsonSerializer.Serialize(resp);
            await context.Response.WriteAsync(json);
        }
        else if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
        {
            context.Response.ContentType = "application/json";
            var resp = new ApiErrorResult<bool>(context.Response.StatusCode, "An error occurred.");
            var json = JsonSerializer.Serialize(resp);
            await context.Response.WriteAsync(json);
        }
    }
}
