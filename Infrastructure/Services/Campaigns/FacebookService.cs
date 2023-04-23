using Application.DTO;
using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Channels;
using Facebook;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Campaigns
{
    public class FacebookService : IActivityServiceAccessor
    {

        private const string AppId = "your_app_id";
        private const string AppSecret = "your_app_secret";
        private const string PageAccessToken = "your_page_access_token";

        public FacebookService(ChannelSetting setting)
        {

        }

        public Task<ServiceMessageBroker> Execute(Activity activity)
        {

            if(activity.DateSchedule == null || activity.DateSchedule <= DateTime.Today)
            {
                
                var client = new FacebookClient(PageAccessToken);
                var result = client.Post("/1947805065548714/feed/", new { activity.Body });
                return Task.FromResult(new ServiceMessageBroker(DateTime.Today, "Success", true));

            }

            return Task.FromResult(new ServiceMessageBroker(DateTime.Today,"",false));

            
        }

        public Task<bool> ForExecute(Activity value)
        {
            throw new NotImplementedException();
        }
    }
}
