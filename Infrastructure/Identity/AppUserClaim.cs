using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity
{
    public class AppUserClaim : IdentityUserClaim<int>
    {
        [NotMapped]
        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public virtual AppUser User { get; set; }
    }
}