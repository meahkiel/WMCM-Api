using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Core.Channels;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Repositories.Unit;

namespace Infrastructure.Services.Campaigns
{
    public class ServiceFactory : IServiceFactory
    {
        
        private readonly IPhotoAccessor photoAccessor;
        private readonly IWebPostService webPostService;
        private readonly ISMSService smsService;
        private readonly ISendSMTPClient sendSMTPClient;

        public ServiceFactory(
            IWebPostService webPostService,
            ISMSService smsService,
            ISendSMTPClient sendSMTPClient)
        {
            
           
            this.webPostService = webPostService;
            this.smsService = smsService;
            this.sendSMTPClient = sendSMTPClient;
        }

        public IActivityServiceAccessor GetSMS(ChannelSetting setting) {
            return new SMSActivityService(smsService, setting);
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
            return new FacebookService(setting);
        }
    }
}
