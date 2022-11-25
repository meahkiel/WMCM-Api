using MediatR;
using SMSService.Application.Common.DTO;
using SMSService.Application.Interfaces;

namespace SMSService.Application.SMSActivities.Commands
{
    public class SendSMS
    {

        public class Command : IRequest<ServiceResult>
        {
            public string Id { get; set; } = null!;
            public string Receipient { get; set; } = null!;
            public string From { get; set; } = null!;

            public string Body { get; set; } = null!;

            public DateTime DateSchedule { get; set; } = DateTime.Today;
        }

        public class CommandHandler : IRequestHandler<Command, ServiceResult>
        {
            private readonly ISMSService _service;

            public CommandHandler(ISMSService service)
            {
                _service = service;
            }
            public async Task<ServiceResult> Handle(Command request, CancellationToken cancellationToken)
            {
                await _service.Execute(request.Body, request.From, request.Receipient);

                return new ServiceResult("1", DateTime.Today, "Success");
            }
        }

    }
}
