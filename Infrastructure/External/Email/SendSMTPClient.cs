using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Infrastructure.External.Email
{

    public interface ISendSMTPClient
    {
        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


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

        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public async Task SendEmailAsync(EmailParameter parameter) {
            try {

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(Email, Email));
                mimeMessage.To.Add(new MailboxAddress(parameter.To, parameter.To));
                mimeMessage.Subject = parameter.Subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = parameter.Body
                };

                await SendProcess(mimeMessage,Host,
                    Port,UserName,Password);
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
