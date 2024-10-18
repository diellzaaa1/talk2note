using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace talk2note.Application.Services.EmailService
{
    public class MailService : IMailService
    {
        private readonly EmailSettings _emailSettings;

        public MailService(IOptions<EmailSettings> emailSettings) 
        {
            _emailSettings = emailSettings.Value; 
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            Console.WriteLine($"Sending email to: {recipientEmail}");

            var client = new SendGridClient(_emailSettings.Password);
            var from = new EmailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            var to = new EmailAddress(recipientEmail);
            var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, message, message);

            var response = await client.SendEmailAsync(emailMessage);

            if (response.StatusCode < HttpStatusCode.OK || response.StatusCode >= HttpStatusCode.MultipleChoices)
            {
                throw new Exception($"Email sending failed with status code: {response.StatusCode}");
            }
        }

    }
}
