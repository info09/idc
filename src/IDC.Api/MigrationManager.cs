using IDC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IDC.Api;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                context.Database.Migrate();
                new DataSeeder().SeedAsync(context).Wait();
            }
        }
        return app;
    }
}
