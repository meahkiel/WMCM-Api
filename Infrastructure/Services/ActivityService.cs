using Core.Campaigns;
using Infrastructure.External.SMS;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services
{

    public interface IActivityService
    {
        Task<Activity> CreateBulkSMS(string title,string description, List<string> mobileNos,string message,DateTime? sendDate);

    }
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
            //start sending the message
            foreach (var mobileNo in mobileNos)
            {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(mobileNo, "", message));
            }

            return Activity.CreateSMSActivity(title, description, message, DateTime.Now, sendDate);

        }

       
    }
}
