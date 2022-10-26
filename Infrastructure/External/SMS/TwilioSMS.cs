using Application.Channels;
using Infrastructure.Core;
using Infrastructure.External.Credential;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public class TwilioSMS : ISMSService
    {
        
        private readonly IMediator _mediator;

        public TwilioSMS(IMediator mediator)
        {
            
            _mediator = mediator;
        }

        public async Task<bool> SendBulkSMSAsync(List<string> tos, string from, string message)
        {
            try {

                var mResult = await _mediator.Send(new GetByType.Query { Type = "twilio" });
                var channel = mResult.Value;

                TwilioClient.Init(channel.ApiKey, channel.ApiSecretKey);
                
                foreach (var to in tos)
                {
                    var result = await MessageResource.CreateAsync(
                                        body: message,
                                        from: new Twilio.Types.PhoneNumber(channel.PhoneNo),
                                        to: new Twilio.Types.PhoneNumber(to));
                }

                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<MessageResource> SendSMS(SMSFormValue value)
        {
            var mResult = await _mediator.Send(new GetByType.Query { Type = "twilio" });
            var channel = mResult.Value;
            TwilioClient.Init(channel.ApiKey, channel.ApiSecretKey);

            var result = await MessageResource.CreateAsync(
                body: value.Message,
                from: new Twilio.Types.PhoneNumber(channel.PhoneNo),
                to: new Twilio.Types.PhoneNumber(value.To));

            return result;
        }
    }
}
