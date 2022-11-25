using Application.SeedWorks;
using Core.Enum;
using MediatR;
using Repositories.Unit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities.Commands
{
    public class DeleteActivity
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string ActivityId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;

            public CommandHandler(UnitWrapper context)
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var campaign = await _context.CampaignRepo.GetByActivity(request.ActivityId);
                    if (campaign == null)
                        throw new Exception("Activity not found");


                    //check if the activity is pending
                    var activity = campaign.Activities.SingleOrDefault();
                    if(activity.Status != ActivityStatusEnum.Pending.ToString())
                    {
                        throw new Exception("Activity is no longer pending and cannot be deleted");
                    }

                    _context.CampaignRepo.DeleteActivity(campaign.Activities.SingleOrDefault());
                    
                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);
                }
                catch(Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);

                }

            }
        }

    }
}
