// File: Models/JwtTokenRequest.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models
{
    /// <summary>
    /// Request model for generating a JWT token.
    /// </summary>
    public class JwtTokenRequest
    {
        /// <summary>
        /// Unique account identifier (e.g., username or email).
        /// </summary>
        [Required(ErrorMessage = "شناسه حساب کاربری الزامی است.")]
        [StringLength(100, ErrorMessage = "شناسه حساب کاربری نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public string AccountId { get; set; } = string.Empty;

        /// <summary>
        /// Role for the token.
        /// </summary>
        [Required(ErrorMessage = "نقش الزامی است.")]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Type of the token (e.g., Access or Refresh).
        /// </summary>
        [Required(ErrorMessage = "نوع توکن الزامی است.")]
        public string TokenType { get; set; } = string.Empty;
    }
}