using Application.Core;
using Application.DTO;
using Application.Interface;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class HandleActivity
    {

        public class Command : IRequest<Result<Unit>> {
            public ActivityEntryDTO Activity { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            private readonly IServiceFactory _serviceFactory;

            public CommandHandler(UnitWrapper context,
                IServiceFactory serviceFactory)
            {
                _context = context;
                _serviceFactory = serviceFactory;
            }

            private IActivityServiceAccessor GetService(string serviceType) => serviceType switch
            {

                "web" => _serviceFactory.GetWebPost(),
                _ => throw new Exception("Cannot Service Type")
            };


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    IActivityServiceAccessor activityService = null;

                    activityService = GetService(request.Activity.Type);

                    var activity = await activityService.Execute(request.Activity);

                    var campaign = await _context.CampaignRepo
                                               .GetSingleCampaign(request.Activity.CampaignId);
                    if (campaign == null)
                        throw new Exception("Campaign not found");

                    campaign.AddActivity(activity);
                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);

                }
                catch(Exception ex) {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }

    }
}
