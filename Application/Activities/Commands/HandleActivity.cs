using Application.Contacts;
using Application.DTO;
using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Contacts;
using Core.Enum;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities.Commands
{
    public class HandleActivity
    {

        public class Command : IRequest<Result<Unit>>
        {
            public ActivityEntryDTO Activity { get; set; }
        }

       

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            private readonly IServiceFactory _serviceFactory;
            private readonly IPhotoAccessor _photoAccessor;

            public CommandHandler(
                UnitWrapper context,
                IServiceFactory serviceFactory,
                IPhotoAccessor photoAccessor) {
                
                _context = context;
                _serviceFactory = serviceFactory;
                _photoAccessor = photoAccessor;
            }

            private async Task<IActivityServiceAccessor> GetService(string serviceType) => serviceType switch
            {

                "web" => _serviceFactory.GetWebPost(await _context.Channels.FindByTypeAsync("web")),
                "sms" => _serviceFactory.GetSMS(await _context.Channels.FindByTypeAsync("twilio")),
                "email" => _serviceFactory.GetSMTP(await _context.Channels.FindByTypeAsync("email")),
                "social" => _serviceFactory.GetSocial(await _context.Channels.FindByTypeAsync("social")),
                _ => throw new Exception("Cannot Service Type")
            };

            private async Task<Activity?> CreateActivity(ActivityEntryDTO dto)
            {
                IEnumerable<Contact> contacts;
                ContactSerialize serialize;

                switch (dto.Type) {
                    case "sms":

                        contacts = await _context.Customers.GetContactByGroup(dto.ToGroup);
                        serialize = new ContactSerialize(contacts.ToList(), dto.To);
                        string strmobileNos = serialize.Serialize("mobile");

                        return Activity.CreateSMSActivity(
                                    dto.Title,
                                    strmobileNos,
                                    dto.Body,
                                    dto.DateToSend ?? DateTime.Today,
                                    dto.DateToSend,
                                    ActivityStatusEnum.Pending);
                    case "email":

                        contacts = await _context.Customers.GetContactByGroup(dto.ToGroup);
                        serialize = new ContactSerialize(contacts.ToList(), dto.To);
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
                        
                    case "web":
                        PhotoUplodResult result = new PhotoUplodResult();
                        if (dto.CoverImageFile != null)
                        {
                            result = await _photoAccessor.AddPhoto(dto.CoverImageFile);
                        }
                        return new Activity
                        {
                            CoverImage = result.Url ?? "",
                            Type = dto.Type,
                            Description = dto.Description,
                            To = dto.To,
                            Body = dto.Body,
                            Title = dto.Title,
                            DispatchDate = DateTime.Today,
                            DateSchedule = DateTime.Today,
                            Status = ActivityStatusEnum.Pending.ToString()
                        };
                       
                    case "social":
                        return new Activity
                        {
                            Type = dto.Type,
                            To = dto.To,
                            Body = dto.Body,
                            Title = dto.Title,
                            DispatchDate = DateTime.Today,
                            DateSchedule = DateTime.Today,
                            Status = ActivityStatusEnum.Pending.ToString()
                        };
                       
                    default:
                        return null;
                }
            }


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {

                    var newActivity = await CreateActivity(request.Activity);
                    var service = await GetService(request.Activity.Type);
                    var response = await service.Execute(newActivity);
                    
                    if(response.IsSuccess)
                    {
                        newActivity.Status = ActivityStatusEnum.Completed.ToString();
                        newActivity.DispatchDate = response.DispatchDate;
                    }

                    var campaign = await _context.CampaignRepo
                                               .GetSingleCampaign(request.Activity.CampaignId);
                    if (campaign == null)
                        throw new Exception("Campaign not found");

                    campaign.AddActivity(newActivity);
                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);

                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }

    }
}
