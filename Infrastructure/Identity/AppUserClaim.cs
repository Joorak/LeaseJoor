using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUserClaim : IdentityUserClaim<int>
    {
        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public virtual AppUser User { get; set; }
    }
}