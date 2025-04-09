using IDC.Application.Services.Interfaces;
using IDC.Application.Services;
using IDC.Domain.ConfigOptions;
using IDC.Domain.Data.Identity;
using IDC.Domain.SeedWorks;
using IDC.Infrastructure.Data;
using IDC.Infrastructure.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using IDC.Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IDC.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    public static void ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
            var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console().ReadFrom.Configuration(context.Configuration)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        });
    }

    public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
        services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
        services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICompanyService, CompanyService>();
    }

    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var apiConfiguration = configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>();
        services.AddSingleton(apiConfiguration);

        return services;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        var configuration = services.GetOptions<ApiConfiguration>("ApiConfiguration");
        if (configuration == null || string.IsNullOrEmpty(configuration.IssuerUri) ||
            string.IsNullOrEmpty(configuration.ApiName)) throw new Exception("ApiConfiguration is not configured!");

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "IDC API V1",
                    Version = configuration.ApiVersion
                });

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration.IdentityServerBaseUrl}/connect/authorize"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "IDP_api.read", "Read Access to IDC API" },
                            { "IDP_api.write", "Write Access to IDC API" }
                        }
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Name = JwtBearerDefaults.AuthenticationScheme
                    },
                    new List<string>
                    {
                        "IDP_api.read",
                        "IDP_api.write"
                    }
                }
            });
        });
    }
}
