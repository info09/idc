﻿using IDC.Domain.Data.Company;
using IDC.Domain.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace IDC.Infrastructure.Data;

public class DataSeeder
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        var passwordHasher = new PasswordHasher<AppUser>();
        var adminRoleId = Guid.NewGuid();
        if (!context.Roles.Any())
        {
            var adminRole = new AppRole()
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                DisplayName = "Administrator",
            };
            context.Roles.Add(adminRole);
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var userAdminId = Guid.NewGuid();

            var userAdmin = new AppUser()
            {
                Id = userAdminId,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                UserName = "sysadmin",
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.UtcNow,
            };
            userAdmin.PasswordHash = passwordHasher.HashPassword(userAdmin, "Admin@123$");
            await context.Users.AddAsync(userAdmin);

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
            {
                RoleId = adminRoleId,
                UserId = userAdminId,
            });
            await context.SaveChangesAsync();
        }

        //if (!context.Companies.Any())
        //{
        //    var companyId = Guid.NewGuid();
        //    var company = new Company()
        //    {
        //        Id = companyId,
        //        Name = "ICSP Việt Nam",
        //        Address = "Ngõ 82 Duy Tân",
        //        Email = "huytq@ics-p.vn",
        //        CompanyType = CompanyType.Trial,
        //        PhoneNumber = "0328478290",
        //        DateCreated = DateTime.UtcNow
        //    };
        //    await context.Companies.AddAsync(company);
        //    await context.SaveChangesAsync();
        //}
    }
}
