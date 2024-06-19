using EventSharing.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EventSharing.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient
            {
                Port = _emailSettings.Port,
                Host = _emailSettings.SmtpServer,
                EnableSsl = _emailSettings.EnabledSsl,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };
            return client.SendMailAsync(_emailSettings.From, email, subject, htmlMessage);
        }
    }
}
