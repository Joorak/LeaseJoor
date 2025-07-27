using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUserToken : IdentityUserToken<int>
    {
        public virtual AppUser User { get; set; }
    }
}