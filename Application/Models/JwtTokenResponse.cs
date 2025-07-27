namespace Application.Models
{
    public class JwtTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string ExpiresIn { get; set; }
    }
}