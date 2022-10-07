using Application.Core;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.Email
{

    public interface ISendSMTPClient
    {
        Task SendEmailAsync(EmailParameter parameter);
    }

    public class EmailParameter {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


    }
    public class SendSMTPClient : ISendSMTPClient
    {

        private EmailSetting _emailSetting;

        public SendSMTPClient(IOptions<EmailSetting> settings)
        {
            _emailSetting = settings.Value;
        }
       
        public async Task SendEmailAsync(EmailParameter parameter) {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSetting.From, _emailSetting.From));
            mimeMessage.To.Add(new MailboxAddress(parameter.To, parameter.To));
            mimeMessage.Subject = parameter.Subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = parameter.Body
            };

            await SendProcess(mimeMessage);

        }

        private async Task SendProcess(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_emailSetting.Host, _emailSetting.Port, false);
                client.Authenticate(_emailSetting.UserName, _emailSetting.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
