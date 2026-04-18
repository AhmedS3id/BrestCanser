using BrestCanser.Api.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BrestCanser.Api.Services;

public class EmailService : IEmailSender
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject
        };

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            _logger.LogInformation("Sending email to {Email}", email);
            await smtp.SendAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending email");
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}