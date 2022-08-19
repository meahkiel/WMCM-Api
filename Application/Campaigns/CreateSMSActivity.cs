using Application.Core;
using Application.DTO;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class CreateSMSActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ActivitySMSDTO SMSActivity { get; set; }

        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IActivityService _activityService;
            private readonly DataContext _context;

            public CommandHandler(IActivityService activityService,DataContext context )
            {
                _activityService = activityService;
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {

                    var activity = await _activityService.CreateSMS(request.SMSActivity.Group,
                                                request.SMSActivity.Message, request.SMSActivity.DateToSend);

                    
                    var campaign = await _context.Campaigns.Where(c => c.Id == request.SMSActivity.CampaignId)
                                    .FirstOrDefaultAsync();
                   
                    if (campaign == null) throw new Exception("campaign doesn't exist");
                    activity.Campaign = campaign;
                    
                    _context.Activities.Add(activity);

                    var result = await _context.SaveChangesAsync() > 0;
                    if(!result)
                    {
                        throw new Exception("Error on campaign");
                    }

                    return Result<Unit>.Success(Unit.Value);

                }catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }

            }
        }


    }
}
