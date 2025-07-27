using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUserRole : IdentityUserRole<int>
    {
        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public virtual AppUser User { get; set; }

        /// <summary>
        /// Navigation property for the associated role. 
        /// </summary>
        public virtual AppRole Role { get; set; }
    }
}