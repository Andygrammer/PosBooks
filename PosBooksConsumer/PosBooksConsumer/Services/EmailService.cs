using System.Net.Mail;
using System.Net;

namespace PosBooksConsumer.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailRecipient, string emailSubject, string emailBody);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string emailRecipient, string emailSubject, string emailBody)
        {
            string smtpServer = _configuration.GetSection("Email")["ServidorSMTP"] ?? string.Empty;
            int smtpPort = int.Parse(_configuration.GetSection("Email")["PortaSMTP"]);
            string smtpUser = _configuration.GetSection("Email")["Usuario"] ?? string.Empty;
            string smtpPass = _configuration.GetSection("Email")["Senha"] ?? string.Empty;

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = false
            };
            mailMessage.To.Add(emailRecipient);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
                throw;
            }
        }
    }
}
