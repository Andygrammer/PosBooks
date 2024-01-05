namespace PosBooksConsumer.Services
{
    public interface IEmailService
    {
        Task SendEmail(string email, string content);
    }

    public class EmailService : IEmailService
    {
        public Task SendEmail(string email, string content)
        {
            throw new NotImplementedException();
        }
    }
}
