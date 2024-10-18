namespace talk2note.Application.Services.EmailService
{
    public interface IMailService
    {
        Task  SendEmailAsync(string toEmail, string subject, string body);
    }
}
