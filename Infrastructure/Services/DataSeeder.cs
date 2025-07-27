using Domain.Constants;
using Domain.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Seed نقش‌ها
            string[] roles = { StringRoleResources.Admin, StringRoleResources.User, StringRoleResources.Customer, StringRoleResources.Supplier };
            foreach (var roleName in roles)
            {
                //if (!roleManager.Roles.Any(r => r.Name == roleName))
                //{
                //    await roleManager.CreateAsync(new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                //}
                var role = await roleManager.FindByNameAsync(roleName);
                if (role is null)
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                }
                //if (!await roleManager.RoleExistsAsync(roleName))
                //{
                //    await roleManager.CreateAsync(new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                //}
            }

            // Seed کاربران
            var users = new[]
            {
                new { UserName = "admin@example.com", FirstName = "Admin", LastName = "User", Password = "Admin@123", Role = StringRoleResources.Admin },
                new { UserName = "user@example.com", FirstName = "Regular", LastName = "User", Password = "User@123", Role = StringRoleResources.User },
                new { UserName = "customer@example.com", FirstName = "Customer", LastName = "User", Password = "Customer@123", Role = StringRoleResources.Customer },
                new { UserName = "supplier@example.com", FirstName = "Supplier", LastName = "User", Password = "Supplier@123", Role = StringRoleResources.Supplier }
            };

            foreach (var userData in users)
            {
                var user = await userManager.FindByNameAsync(userData.UserName);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = userData.UserName,
                        Email = userData.UserName,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        IsActive = true,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(user, userData.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userData.Role);
                    }
                }
            }
        }
    }
}