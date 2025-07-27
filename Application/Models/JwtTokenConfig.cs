// File: Models/JwtTokenConfig.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Configuration settings for JWT tokens.
    /// </summary>
    public class JwtTokenConfig
    {
        /// <summary>
        /// Secret key for signing JWT tokens.
        /// </summary>
        [Required(ErrorMessage = "کلید مخفی JWT الزامی است.")]
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Issuer of the JWT token.
        /// </summary>
        [Required(ErrorMessage = "صادرکننده JWT الزامی است.")]
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audience of the JWT token.
        /// </summary>
        [Required(ErrorMessage = "مخاطب JWT الزامی است.")]
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Access token value.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Refresh token value.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}