using MimeKit;
using SMSService.Application.Common;
using SMSService.Application.Interfaces;
using Microsoft.Extensions.Options;
using SMSService.Application.Common.Settings;
using MailKit.Net.Smtp;

namespace SMSService.Infrastructure.Services
{
    public class SendSMTPClient : ISendSMTPClient
    {

        private readonly string _emailProvider;
        private readonly string _userNameProvider;
        private readonly string _passwordProvider;
        private readonly string _host;
        private readonly int _port;

       
        public SendSMTPClient(IOptions<SMTPSettings> options)
        {
            
            _emailProvider = options.Value.Email;
            _userNameProvider = options.Value.UserName;
            _passwordProvider = options.Value.Password;
            _host = options.Value.Host;
            _port = options.Value.Port;
        }

        public async Task SendMailAsync(EmailParameter parameter)
        {
            try
            {

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailProvider, _emailProvider));
                mimeMessage.To.Add(new MailboxAddress(parameter.To, parameter.To));
                mimeMessage.Subject = parameter.Subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = parameter.Body
                };

                await SendProcess(mimeMessage, _host,
                    _port, _userNameProvider, _passwordProvider);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SendProcess(MimeMessage mimeMessage, string host,
           int port, string username, string password)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(host, port, false);
                client.Authenticate(username, password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
