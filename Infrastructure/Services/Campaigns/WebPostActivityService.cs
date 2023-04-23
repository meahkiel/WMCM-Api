using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Channels;
using Infrastructure.External.Web;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Campaigns
{
    public class WebPostActivityService : IActivityServiceAccessor
    {
        
        private readonly IWebPostService _webPostService;
        private readonly ChannelSetting _setting;

        public WebPostActivityService(
            IWebPostService webPostService,
            ChannelSetting setting
            )
        {
           
            _webPostService = webPostService;
            _setting = setting;
        }

       
        

        public async Task<ServiceMessageBroker?> Execute(Activity activity)
        {

            _webPostService.BaseUrl = _setting.BaseUrl;

            await _webPostService.Post(new Core.PostParameter
            {
                BackgroundImage = activity.CoverImage,
                HtmlBody = activity.Body,
                SectionName = activity.To,
                Title = activity.Title,
                LaunchDate = activity.DateSchedule ?? DateTime.Now,
                EndDate = activity.DateSchedule == null ? DateTime.Now.AddDays(15) : activity.DateSchedule.Value.AddDays(15)
            });

            return new ServiceMessageBroker(DateTime.Today, "Success", true);
        }

        
    }
}
