using Application.Contacts;
using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Core.Enum;
using Infrastructure.External.Email;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Campaigns
{
    public class SMTPActivityService : IActivityServiceAccessor<ActivityEntryDTO,Activity>
    {

        private readonly ISendSMTPClient _sendSMTPClient;
        private readonly UnitWrapper _context;

        public SMTPActivityService(ISendSMTPClient sendSMTPClient, UnitWrapper context)
        {
            _sendSMTPClient = sendSMTPClient;
            _context = context;
        }

        public async Task<Activity> Execute(ActivityEntryDTO dto)
        {
           
            var activity = await CreateSMTPActivity(dto);
            var smsActivity = await _context.Channels.FindByTypeAsync("email");

            activity.Status = ActivityStatusEnum.Pending.ToString();
            activity.DispatchDate = dto.DateToSend ?? DateTime.Now;
            if (CanExecute(dto))
            {
                List<string> emailAddress = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await Execution(emailAddress, dto.Subject, dto.Body);
                activity.Status = ActivityStatusEnum.Completed.ToString();
            }

            return activity;
        }

        private async Task<List<string>> GetEmails(ActivityEntryDTO dto)
        {
            var contacts = await _context.Customers.GetContactByGroup(dto.ToGroup);
            ContactSerialize serialize = new ContactSerialize(contacts.ToList(), dto.To);
            List<string> emailAddress = serialize.ExtractEmail();
            return emailAddress;
        }

        public async Task Execution(List<string> emailAdds, string subject, string body)
        {
            var smsActivity = await _context.Channels.FindByTypeAsync("email");

            _sendSMTPClient.Email = smsActivity.Email;
            _sendSMTPClient.Port = smsActivity.Port;
            _sendSMTPClient.Host = smsActivity.Host;
            _sendSMTPClient.UserName = smsActivity.UserName;
            _sendSMTPClient.Password = smsActivity.Password;

            foreach (var email in emailAdds)
            {
                var emailParam = new EmailParameter
                {
                    Subject = subject,
                    Body = body,
                    To = email
                };

                await _sendSMTPClient
                            .SendEmailAsync(emailParam);
            }
        }

        public async Task<Activity> ForExecute(ActivityEntryDTO dto)
        {
            var activity = (await _context.CampaignRepo.GetByActivity(dto.Id)).Activities.SingleOrDefault();
            if (activity == null)
                throw new Exception("Activity not found");

            var smsActivity = await _context.Channels.FindByTypeAsync("email");

            activity.Status = ActivityStatusEnum.Pending.ToString();
            activity.DispatchDate = dto.DateToSend ?? DateTime.Now;
            if (CanExecute(dto))
            {
                List<string> emailAddress = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await Execution(emailAddress, dto.Subject, dto.Body);
                activity.Status = ActivityStatusEnum.Completed.ToString();
            }

            return activity;
        }

       
        public bool CanExecute(ActivityEntryDTO dto)
        {
            return dto.DateToSend == null || dto.DateToSend <= DateTime.Today;
        }

        private async Task<Activity> CreateSMTPActivity(ActivityEntryDTO dto)
        {

            var contacts = await _context.Customers.GetContactByGroup(dto.ToGroup);
            ContactSerialize serialize = new ContactSerialize(contacts.ToList(), dto.To);
            string strEmail = serialize.Serialize("email");
            string description = dto.ToGroup +
                            (!string.IsNullOrEmpty(dto.ToGroup) ? dto.ToGroup : "");

            return Activity.CreateEmailActivity(
                  dto.Title,
                  description,
                  dto.Subject,
                  dto.Body,
                  dto.DateToSend ?? DateTime.Today,
                  strEmail, dto.DateToSend,
                  ActivityStatusEnum.Pending);
        }
    }
}
