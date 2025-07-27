// File: Models/EmailSettings.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Configuration settings for sending emails.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// SMTP server host.
        /// </summary>
        [Required(ErrorMessage = "هاست SMTP الزامی است.")]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// SMTP server port.
        /// </summary>
        [Range(1, 65535, ErrorMessage = "پورت باید بین 1 و 65535 باشد.")]
        public int Port { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        [StringLength(200, ErrorMessage = "موضوع ایمیل نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
        public string? Subject { get; set; }

        /// <summary>
        /// Email message body.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// SMTP username for authentication.
        /// </summary>
        [Required(ErrorMessage = "نام کاربری SMTP الزامی است.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// SMTP password for authentication.
        /// </summary>
        [Required(ErrorMessage = "رمز عبور SMTP الزامی است.")]
        public string Password { get; set; } = string.Empty;
    }
}