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
        Task<Activity> CreateSMS(string title,string group,string message,DateTime? sendDate);
    }
    public class ActivityService : IActivityService
    {
        private readonly DataContext _context;
        private readonly ISMSService _smsService;

        public ActivityService(DataContext context,
            ISMSService smsService)
        {
            _context = context;
            _smsService = smsService;
        }


        public async Task<Activity> CreateSMS(string title,string group, string message, DateTime? sendDate)
        {


            var contacts = await _context.Contacts
                        .Where(c => c.GroupTag.Contains(group))
                        .AsNoTracking()
                        .ToListAsync();

            if (!contacts.Any())
                throw new Exception("Contact cannot find");

            List<MessageResource> messageResources = new List<MessageResource>();
            //start sending the message
            foreach (var contact in contacts)
            {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(contact.PrimaryContact,"",message));
            }
                
            return Activity.CreateSMSActivity(title, group, message,DateTime.Now, sendDate);
        }
    }
}
