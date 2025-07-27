using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUser : IdentityUser<int>
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Full name of the user (computed property).
        /// </summary>
        public string? FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Indicates if the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Navigation property for user tokens.
        /// </summary>
        public virtual ICollection<AppUserToken> UserTokens { get; set; } = new List<AppUserToken>();

        /// <summary>
        /// Navigation property for user roles.
        /// </summary>
        public virtual ICollection<AppUserRole> Roles { get; set; } = new List<AppUserRole>();

        /// <summary>
        /// Navigation property for user logins.
        /// </summary>
        public virtual ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();

        /// <summary>
        /// Navigation property for user claims.
        /// </summary>
        public virtual ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();
    }
}