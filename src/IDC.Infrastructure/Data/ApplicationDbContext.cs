using IDC.Domain.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDC.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(i => i.Id);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(i => i.Id);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(i => i.UserId);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(i => new { i.UserId, i.RoleId });
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
    }
}
