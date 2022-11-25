using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Repositories.Unit;

namespace Infrastructure.Services.Campaigns
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly UnitWrapper context;
        private readonly IPhotoAccessor photoAccessor;
        private readonly IWebPostService webPostService;
        private readonly ISMSService smsService;

        public ServiceFactory(UnitWrapper context,
            IPhotoAccessor photoAccessor,
            IWebPostService webPostService,
            ISMSService smsService)
        {
            this.context = context;
            this.photoAccessor = photoAccessor;
            this.webPostService = webPostService;
            this.smsService = smsService;
        }

        public IActivityServiceAccessor<ActivityEntryDTO,Activity> GetSMS()
        {
            return new SMSActivityService(smsService, context);
        }

        public IActivityServiceAccessor<ActivityEntryDTO, Activity> GetWebPost()
        {
            return new WebPostActivityService(context, webPostService, photoAccessor);
        }
    }
}
