using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string roleName) : base(roleName)
        {
            NormalizedName = roleName.ToUpperInvariant();
        }
        public AppRole() : base()
        {
            
        }
        /// <summary>
        /// Navigation property for users in this role.
        /// </summary>
        public virtual ICollection<AppUserRole> Users { get; set; } = new List<AppUserRole>();

        /// <summary>
        /// Navigation property for claims associated with this role.
        /// </summary>
        public virtual ICollection<AppRoleClaim> Claims { get; set; } = new List<AppRoleClaim>();
    }
}