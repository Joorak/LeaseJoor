// File: Interfaces/IEmailService.cs
using Application.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email with the specified settings.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="mail">The email settings.</param>
        Task SendEmail(string email, EmailSettings mail);
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}