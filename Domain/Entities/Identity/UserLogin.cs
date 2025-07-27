namespace Domain.Entities.Identity
{
    public class UserLogin
    {
        public string LoginProvider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public string? ProviderDisplayName { get; set; }
        public int UserId { get; set; }
    }
}