namespace Sauvio.Services.Email
{
    public interface IEmailService
    {
        void SendConfirmationEmail(string toEmail, string token);
    }
}
