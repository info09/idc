using IDC.Infrastructure.Data;

namespace IDC.Api.Extensions;

public static class HostExtensions
{
    public static void AddApplicationConfigurations(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

    }
}
