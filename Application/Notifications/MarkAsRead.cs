using Application.SeedWorks;

namespace Application.Notifications
{
    public class MarkAsRead
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string NotificationId { get; set; }
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
                    var notification = await _context.Notifications.GetUnread(request.NotificationId);

                    if (notification == null)
                        throw new Exception("Notification not found");

                    notification.HasRead = true;
                    _context.Notifications.Update(notification);

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
