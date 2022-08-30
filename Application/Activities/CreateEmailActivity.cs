using Application.DTO;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class CreateEmailActivity
    {
        public class Command : IRequest<Unit>
        {
            public ActivitySMSDTO SMSActivity { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
