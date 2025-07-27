using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity
{
    public class AppRoleClaim : IdentityRoleClaim<int>
    {
        [NotMapped]
        /// <summary>
        /// Navigation property for the associated role.
        /// </summary>
        public virtual AppRole Role { get; set; }
    }
}