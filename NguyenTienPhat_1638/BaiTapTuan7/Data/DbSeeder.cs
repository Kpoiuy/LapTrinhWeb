using BaiTapTuan7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTuan7.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await context.Database.EnsureCreatedAsync();

        await EnsureRolesAsync(roleManager);
        await EnsureAdminUserAsync(userManager);
        await EnsureStudentUserAsync(userManager);
    }

    private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { RoleNames.Admin, RoleNames.Student };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task EnsureAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        const string adminUserName = "admin01";
        const string adminEmail = "admin01@university.edu.vn";
        const string adminPassword = "Admin@123";

        var admin = await userManager.FindByNameAsync(adminUserName);
        if (admin is null)
        {
            admin = new IdentityUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);
            if (!createResult.Succeeded)
            {
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(admin, RoleNames.Admin))
        {
            await userManager.AddToRoleAsync(admin, RoleNames.Admin);
        }
    }

    private static async Task EnsureStudentUserAsync(UserManager<IdentityUser> userManager)
    {
        const string studentUserName = "student01";
        const string studentEmail = "student01@university.edu.vn";
        const string studentPassword = "Student@123";

        var student = await userManager.FindByNameAsync(studentUserName);
        if (student is null)
        {
            student = new IdentityUser
            {
                UserName = studentUserName,
                Email = studentEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(student, studentPassword);
            if (!createResult.Succeeded)
            {
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(student, RoleNames.Student))
        {
            await userManager.AddToRoleAsync(student, RoleNames.Student);
        }
    }
}
