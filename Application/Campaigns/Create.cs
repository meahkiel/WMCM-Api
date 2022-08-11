using Core.Campaigns;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {

            public Campaign Campaign { get; set; }

        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;

            public CommandHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //request.Campaign.Id = Guid.NewGuid();
                _context.Campaigns.Add(request.Campaign);
                
                var result = await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
