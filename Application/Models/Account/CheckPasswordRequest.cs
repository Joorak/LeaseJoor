namespace Application.Models
{
    public class CheckPasswordRequest
    {
        public int UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}