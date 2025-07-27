using Domain.Entities.Identity;
using Infrastructure.Identity;

namespace Infrastructure.Services
{
    public static class IdentityMappingService
    {
        public static AppUser ToAppUser(this User user)
        {
            return new AppUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
        }
        public static User ToDomainUser(this AppUser appUser)
        {
            return new User
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                IsActive = appUser.IsActive,
                PasswordHash = appUser.PasswordHash,
                PhoneNumber = appUser.PhoneNumber,
                PhoneNumberConfirmed = appUser.PhoneNumberConfirmed
            };
        }

        public static AppRole ToAppRole(this Role role)
        {
            return new AppRole
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };
        }

        public static Role ToDomainRole(this AppRole appRole)
        {
            return new Role
            {
                Id = appRole.Id,
                Name = appRole.Name,
                NormalizedName = appRole.NormalizedName
            };
        }
    }
}