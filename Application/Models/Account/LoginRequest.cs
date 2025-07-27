namespace Application.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string? PassKey { get; set; }
        public string RoleForLogin { get; set; } = string.Empty;
    }
}