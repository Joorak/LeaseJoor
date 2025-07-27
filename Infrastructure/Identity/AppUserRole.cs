using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity
{
    public class AppUserRole : IdentityUserRole<int>
    {
        [NotMapped]
        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public virtual AppUser User { get; set; }

        [NotMapped]
        /// <summary>
        /// Navigation property for the associated role. 
        /// </summary>
        public virtual AppRole Role { get; set; }
    }
}