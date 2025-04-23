using MailKit.Net.Smtp;
using MimeKit;
using Chaski.Application.Services.Email;
using Microsoft.Extensions.Configuration;

namespace Chaski.Infrastructure.Services;

public class EmailService: IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendEmailConfirmationAsync(string email, string username, string confirmationLink)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _configuration["EmailSettings:SenderName"] ?? "Tu E-commerce", 
            emailSettings["From"]));
        
        message.To.Add(new MailboxAddress(username, email));
        message.Subject = "Confirma tu correo electrónico";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <h1>Bienvenido {username} a nuestro e-commerce</h1>
                <p>Por favor confirma tu correo electrónico haciendo clic en el siguiente enlace:</p>
                <p><a href='{confirmationLink}'>Confirmar correo electrónico</a></p>
                <p>Este enlace expirará en 24 horas.</p>
                <p>Si no solicitaste este registro, por favor ignora este mensaje.</p>"
        };

        message.Body = bodyBuilder.ToMessageBody();

        await SendEmailAsync(message);
    }

    public async Task SendPasswordResetEmailAsync(string email, string username, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _configuration["EmailSettings:SenderName"] ?? "Tu E-commerce", 
            _configuration["EmailSettings:From"]));
        
        message.To.Add(new MailboxAddress(username, email));
        message.Subject = "Restablecer tu contraseña";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <h1>Restablecimiento de contraseña</h1>
                <p>Hola {username},</p>
                <p>Para restablecer tu contraseña, haz clic en el siguiente enlace:</p>
                <p><a href='{resetLink}'>Restablecer contraseña</a></p>
                <p>Si no solicitaste este cambio, por favor ignora este mensaje.</p>"
        };

        message.Body = bodyBuilder.ToMessageBody();

        await SendEmailAsync(message);
    }
    
    private async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
    
        var emailSettings = _configuration.GetSection("EmailSettings");
    
        await client.ConnectAsync(
            emailSettings["SmtpServer"], 
            int.Parse(emailSettings["Port"]), 
            MailKit.Security.SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            emailSettings["Username"], 
            emailSettings["Password"]);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}