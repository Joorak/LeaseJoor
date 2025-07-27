using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUserLogin : IdentityUserLogin<int>
    {
        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public virtual AppUser User { get; set; }
    }
}