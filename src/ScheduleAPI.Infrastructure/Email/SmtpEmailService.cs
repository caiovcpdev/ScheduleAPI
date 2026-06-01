using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ScheduleAPI.Application.Interfaces;


namespace ScheduleAPI.Infrastructure.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public SmtpEmailService(IOptions<EmailSettings> settings) => _settings = settings.Value;

        public async Task EnviarAsync(string destinatario, string nome, string assunto, string corpo)
        {
            var mensagem = new MimeMessage();

            mensagem.From.Add(new MailboxAddress(_settings.NomeRequerente, _settings.EmailRequerente));
            mensagem.To.Add(new MailboxAddress(nome, destinatario));
            mensagem.Subject = assunto;

            mensagem.Body = new TextPart("html") { Text = corpo };

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);


            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(mensagem);
            await client.DisconnectAsync(true);
        }
    }
}
