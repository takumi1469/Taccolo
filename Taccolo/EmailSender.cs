using SendGrid.Helpers.Mail;
using SendGrid;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Taccolo
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridApiKey;

        public EmailSender(IConfiguration configuration)
        {
            // Fetch the API Key from appsettings.json
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(_sendGridApiKey))
            {
                throw new System.Exception("SendGrid API key is missing. Check your configuration.");
            }

            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("takumi1469@gmail.com", "Taccolo Support");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            var response = await client.SendEmailAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error (optional logging)
                var error = await response.Body.ReadAsStringAsync();
                throw new System.Exception($"Failed to send email: {error}");
            }
        }
    }
}



