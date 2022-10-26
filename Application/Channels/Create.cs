using Application.Core;
using Core.Channels;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Channels
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ChannelSetting Value { get; set; }

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
                    
                    _context.Channels.Add(request.Value);

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
