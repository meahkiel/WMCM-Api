using Application.Configuration;
using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Core.Channels;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Microsoft.Extensions.Options;
using Repositories.Unit;
using System.Net.Http;

namespace Infrastructure.Services.Campaigns
{
    public class ServiceFactory : IServiceFactory
    {
        
        private readonly IPhotoAccessor photoAccessor;
        private readonly IWebPostService webPostService;
        private readonly ISMSService smsService;
        private readonly ISendSMTPClient sendSMTPClient;
        private readonly IOptions<TwilioOption> options;
        private readonly HttpClient _httpClient;

        public ServiceFactory(
            IWebPostService webPostService,
            ISMSService smsService,
            ISendSMTPClient sendSMTPClient,
            IOptions<TwilioOption> options,
            HttpClient httpClient)
        {
            
           
            this.webPostService = webPostService;
            this.smsService = smsService;
            this.sendSMTPClient = sendSMTPClient;
            this.options = options;
            _httpClient = httpClient;
        }

        public IActivityServiceAccessor GetSMS(ChannelSetting setting) {
            return new SMSActivityService(smsService, setting,options);
        }

        public IActivityServiceAccessor GetWebPost(ChannelSetting setting)
        {
            return new WebPostActivityService(webPostService,setting);
        }

        public IActivityServiceAccessor GetSMTP(ChannelSetting setting)
        {
            return new SMTPActivityService(sendSMTPClient, setting);
        }

        public IActivityServiceAccessor GetSocial(ChannelSetting setting)
        {
            return new SocialActivityService(_httpClient,setting);
        }
    }
}
