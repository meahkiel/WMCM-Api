using Application.Channels;
using Application.Core;
using MailKit.Net.Smtp;
using MediatR;
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

        
        private readonly IMediator _mediator;

        public SendSMTPClient(IMediator mediator)
        {
            
            _mediator = mediator;
        }
       
        public async Task SendEmailAsync(EmailParameter parameter) {

            try
            {

                var mimeMessage = new MimeMessage();
                var result = await _mediator.Send(new GetByType.Query{ Type = "email" });
                var channel = result.Value;


                mimeMessage.From.Add(new MailboxAddress(channel.Email, channel.Email));
                mimeMessage.To.Add(new MailboxAddress(parameter.To, parameter.To));
                mimeMessage.Subject = parameter.Subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = parameter.Body
                };

                await SendProcess(mimeMessage,channel.Host,
                    channel.Port,channel.UserName,channel.Password);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async Task SendProcess(MimeMessage mimeMessage,string host,
            int port,string username,string password)
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
