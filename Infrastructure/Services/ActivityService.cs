using Application.Core;
using Core.Campaigns;
using Infrastructure.External.SMS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services
{

    public class ActivityService : IActivityService
    {
       
        private readonly ISMSService _smsService;

        public ActivityService(ISMSService smsService)
        {   
            _smsService = smsService;
        }

        public async Task<Activity> CreateBulkSMS(string title, string description, List<string> mobileNos, string message, DateTime? sendDate)
        {
            List<MessageResource> messageResources = new List<MessageResource>();
            
            Activity activity = null;
            if(sendDate.HasValue && sendDate.Value > DateTime.Now)
            {
                activity = Activity.CreateSMSActivity(title, description, message, DateTime.Now, sendDate);
                activity.Status = "pending";
            } 
            else
            {
                //start sending the message
                foreach (var mobileNo in mobileNos)
                {
                    MessageResource messageBroker = await _smsService
                                .SendSMS(new Core.SMSFormValue(mobileNo, "", message));
                }
                activity = Activity.CreateSMSActivity(title, description, message, DateTime.Now, sendDate);
            }

            return activity;
        }

       
    }
}
