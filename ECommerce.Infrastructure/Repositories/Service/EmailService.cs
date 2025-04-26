using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ECommerce.Core.DTOs;
using ECommerce.Core.Services;

namespace ECommerce.Infrastructure.Repositories.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            using (var client = new SmtpClient(_emailSettings.Smtp, _emailSettings.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.From),
                    Subject = emailDTO.Subject,
                    Body = emailDTO.Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(emailDTO.To);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
