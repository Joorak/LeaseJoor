using Domain.Entities.Identity;
using Domain.SeedData;

namespace Infrastructure.Persistence
{
    public static class IdentityDbSeed
    {
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager, RolesSeedModel seedData)
        {
            var roles = new[]
            {
                new Role { Name = seedData.AdminRoleName, NormalizedName = seedData.AdminRoleNormalizedName },
                new Role { Name = seedData.CustomerRoleName, NormalizedName = seedData.CustomerRoleNormalizedName },
                new Role { Name = seedData.SupplierRoleName, NormalizedName = seedData.SupplierRoleNormalizedName },
                new Role { Name = seedData.DefaultRoleName, NormalizedName = seedData.DefaultRoleNormalizedName },
                new Role { Name = seedData.UserRoleName, NormalizedName = seedData.UserRoleNormalizedName }
            };

            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role.Name))
                {
                    await roleManager.CreateAsync(role).ConfigureAwait(false);
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<Role> roleManager, AdminSeedModel seedData)
        {
            var admin = new User
            {
                UserName = $"{seedData.FirstName}@{seedData.LastName}",
                Email = seedData.Email,
                FirstName = seedData.FirstName,
                LastName = seedData.LastName,
                IsActive = true,
            };

            var adminRole = await roleManager.FindByNameAsync(seedData.RoleName).ConfigureAwait(false);
            if (adminRole == null)
            {
                throw new Exception("The selected role does not exist.");
            }

            if (!userManager.Users.Any(u => u.Email == admin.Email))
            {
                await userManager.CreateAsync(admin, seedData.Password!).ConfigureAwait(false);
                await userManager.AddToRoleAsync(admin, adminRole.Name!).ConfigureAwait(false);
            }
        }
    }
}