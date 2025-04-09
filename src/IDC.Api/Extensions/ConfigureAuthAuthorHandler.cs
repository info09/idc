using IDC.Shared.Configurations;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IDC.Api.Extensions;

public static class ConfigureAuthAuthorHandler
{
    public static void ConfigureAuthenticationHandler(this IServiceCollection services)
    {
        var configuration = services.GetOptions<ApiConfiguration>("ApiConfiguration");

        if (configuration == null || string.IsNullOrEmpty(configuration.IssuerUri) || string.IsNullOrEmpty(configuration.ApiName))
        {
            throw new ArgumentNullException($"{nameof(ApiConfiguration)} is not configured properly");
        }
        var issuerUri = configuration.IssuerUri;
        var apiName = configuration.ApiName;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddIdentityServerAuthentication(opt =>
        {
            opt.Authority = issuerUri;
            opt.ApiName = apiName;
            opt.RequireHttpsMetadata = false;
            opt.SupportedTokens = SupportedTokens.Both;
        });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityServerAuthenticationDefaults.AuthenticationScheme, policy =>
            {
                policy.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
    }
}
