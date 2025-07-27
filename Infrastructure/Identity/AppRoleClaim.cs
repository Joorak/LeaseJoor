using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppRoleClaim : IdentityRoleClaim<int>
    {
        /// <summary>
        /// Navigation property for the associated role.
        /// </summary>
        public virtual AppRole Role { get; set; }
    }
}