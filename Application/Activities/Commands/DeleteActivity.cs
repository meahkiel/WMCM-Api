using Application.SeedWorks;
using Core.Enum;

namespace Application.Activities.Commands
{
    public class DeleteActivity
    {
        public record Command(string ActivityId) : IRequest<Result<Unit>>;

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
                    var activity = await _context.CampaignRepo.GetByActivity(request.ActivityId);
                    if (activity == null)
                        throw new Exception("Activity not found");

                    _context.CampaignRepo.DeleteActivity(activity);
                    
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
