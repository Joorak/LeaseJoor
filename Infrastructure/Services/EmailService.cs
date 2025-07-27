using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App", "no-reply@yourapp.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.yourserver.com", 587, false).ConfigureAwait(false);
            await client.AuthenticateAsync("your-smtp-username", "your-smtp-password").ConfigureAwait(false);
            await client.SendAsync(message).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
        public async Task SendEmail(string? email, EmailSettings mail)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "The email should not be null or empty.");
            }

            using var client = new System.Net.Mail.SmtpClient
            {
                Credentials = new NetworkCredential(mail.Username, mail.Password),
                Host = mail.Host,
                Port = mail.Port,
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                To = { new MailAddress(email) },
                From = new MailAddress(mail.Username),
                Subject = mail.Subject,
                Body = mail.Message,
                Priority = MailPriority.High
            };

            await client.SendMailAsync(mailMessage).ConfigureAwait(false);
        }
    }
}