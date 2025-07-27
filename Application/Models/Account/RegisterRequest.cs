namespace Application.Models
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RoleForRegister { get; set; }
    }
}