namespace Chaski.Application.Services.Email;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string username, string confirmationLink);
    Task SendPasswordResetEmailAsync(string email, string username, string resetLink);
}