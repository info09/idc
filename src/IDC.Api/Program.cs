using IDC.Api;
using IDC.Api.Extensions;
using IDC.Application.Services;
using IDC.Application.Services.Interfaces;
using IDC.Domain.ConfigOptions;
using IDC.Domain.Data.Identity;
using IDC.Domain.SeedWorks;
using IDC.Infrastructure.Data;
using IDC.Infrastructure.Repositories;
using IDC.Infrastructure.SeedWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

Log.Information("Starting up the application...");

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");

try
{
    builder.Host.AddApplicationConfigurations();
    builder.Host.ConfigureSerilog();
    builder.Services.ConfigureCors();

    // Add services to the container.

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    builder.Services.ConfigureIdentityServer(configuration);

    builder.Services.ConfigureServices(configuration);

    // Business services and repositories
    var services = typeof(CompanyRepository).Assembly.GetTypes()
        .Where(i => i.GetInterfaces().Any(i => i.Name == typeof(IRepositoryBase<,>).Name) &&
                    !i.IsAbstract &&
                    i.IsClass &&
                    !i.IsGenericType);

    foreach (var service in services)
    {
        var allInterfaces = service.GetInterfaces();
        var directInterface = allInterfaces.Except(allInterfaces.SelectMany(i => i.GetInterfaces())).FirstOrDefault();
        if (directInterface != null)
        {
            builder.Services.Add(new ServiceDescriptor(directInterface, service, ServiceLifetime.Scoped));
        }
    }

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger();

    builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;

        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(0),
            ValidIssuer = configuration["JwtTokenSettings:Issuer"],
            ValidAudience = configuration["JwtTokenSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:Key"]!))
        };
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseErrorWrapping();

    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MigrateDatabase();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}


