using Application.Contacts;
using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Core.Channels;
using Core.Enum;
using Infrastructure.External.SMS;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services.Campaigns
{
    public class SMSActivityService : IActivityServiceAccessor<ActivityEntryDTO,Activity>
    {

        private readonly ISMSService _smsService;
        private readonly UnitWrapper _context;

        public SMSActivityService(ISMSService smsService, UnitWrapper context)
        {
            _smsService = smsService;
            _context = context;
        }

        public bool CanExecute(ActivityEntryDTO dto)
        {
            return dto.DateToSend == null || dto.DateToSend <= DateTime.Today;
        }

        public async Task<Activity> Execute(ActivityEntryDTO dto)
        {

            var activity = await CreateSMSActivity(dto);
            var smsActivity = await _context.Channels.FindByTypeAsync("sms");

            activity.Status = ActivityStatusEnum.Pending.ToString();
            activity.DispatchDate = dto.DateToSend ?? DateTime.Now;
            
            if (CanExecute(dto))
            {
                List<string> mobileNos = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await SendSMS(activity.Body, mobileNos, smsActivity);
                activity.Status = ActivityStatusEnum.Completed.ToString();
            }

            activity.Description = dto.ToGroup +
                                (!string.IsNullOrEmpty(dto.ToGroup) ? dto.ToGroup : "");
            return activity;

        }

        public async Task<Activity> ForExecute(ActivityEntryDTO dto)
        {
            var activity = (await _context.CampaignRepo.GetByActivity(dto.Id)).Activities.SingleOrDefault();
            if (activity == null)
                throw new Exception("Activity not found");

            var smsActivity = await _context.Channels.FindByTypeAsync("sms");

            activity.Status = ActivityStatusEnum.Pending.ToString();
            activity.DispatchDate = dto.DateToSend ?? DateTime.Now;

            if (CanExecute(dto))
            {
                List<string> mobileNos = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await SendSMS(activity.Body, mobileNos, smsActivity);
                activity.Status = ActivityStatusEnum.Completed.ToString();
            }

            activity.Description = dto.ToGroup +
                                (!string.IsNullOrEmpty(dto.ToGroup) ? dto.ToGroup : "");
            return activity;

        }

        private async Task SendSMS(string body, List<string> mobileNos, ChannelSetting smsActivity)
        {
            _smsService.ApiKey = smsActivity.ApiKey;
            _smsService.ApiSecret = smsActivity.ApiSecretKey;

            //start sending the message
            foreach (var mobileNo in mobileNos)
            {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(mobileNo, "",body));
            }
        }

        private async Task<Activity> CreateSMSActivity(ActivityEntryDTO dto)
        {

            var contacts = await _context.Customers.GetContactByGroup(dto.ToGroup);
            ContactSerialize serialize = new ContactSerialize(contacts.ToList(), dto.ToGroup);
            string strmobileNos = serialize.Serialize("mobile");

            return Activity.CreateSMSActivity(
                  dto.Title,
                  strmobileNos,
                  dto.Body,
                  dto.DateToSend ?? DateTime.Today,
                  dto.DateToSend,
                  ActivityStatusEnum.Pending);
        }

        
    }
}
