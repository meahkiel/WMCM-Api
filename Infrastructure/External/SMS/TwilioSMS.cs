using Application.Channels;
using Infrastructure.Core;
using Infrastructure.External.Credential;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public class TwilioSMS : ISMSService
    {
        
        private readonly IMediator _mediator;
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
       


        public async Task<MessageResource> SendSMS(SMSFormValue value)
        {
          
            TwilioClient.Init(ApiKey, ApiSecret);

            var result = await MessageResource.CreateAsync(
                body: value.Message,
                from: new Twilio.Types.PhoneNumber(value.From),
                to: new Twilio.Types.PhoneNumber(value.To));

            return result;
        }
    }
}
