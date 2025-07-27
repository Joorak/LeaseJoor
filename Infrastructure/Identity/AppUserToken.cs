using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity
{
    public class AppUserToken : IdentityUserToken<int>
    {
        [NotMapped]
        public virtual AppUser User { get; set; }
    }
}