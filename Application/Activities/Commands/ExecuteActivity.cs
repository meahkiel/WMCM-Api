using Application.Interface;
using Application.SeedWorks;
using Core.Enum;


namespace Application.Activities.Commands
{
    public class ExecuteActivity
    {
        public record Command(string Id) : IRequest<Result<Unit>>;

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IServiceFactory _serviceFactory;
            private readonly UnitWrapper _context;

            public CommandHandler(IServiceFactory serviceFactory, UnitWrapper context) {
                
                _serviceFactory = serviceFactory;
                _context = context;
            }
            private async Task<IActivityServiceAccessor> GetService(string serviceType) => serviceType switch
            {

                "web" => _serviceFactory.GetWebPost(await _context.Channels.FindByTypeAsync("web")),
                "sms" => _serviceFactory.GetSMS(await _context.Channels.FindByTypeAsync("twilio")),
                "email" => _serviceFactory.GetSMTP(await _context.Channels.FindByTypeAsync("email")),
                _ => throw new Exception("Cannot Service Type")
            };

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    
                    var activity = await _context.CampaignRepo.GetByActivity(request.Id);
                    if (activity == null)
                        throw new Exception("Not found activity");

                    var activityService = await GetService(activity.Type);
                    var response = await activityService.Execute(activity);
                    
                    if(response.IsSuccess) {
                        activity.DispatchDate = response.DispatchDate;
                        activity.Status = ActivityStatusEnum.Completed.ToString();
                    }

                    _context.CampaignRepo.UpdateActivity(activity);
                    
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
                throw new System.NotImplementedException();
            }
        }

    }
}
