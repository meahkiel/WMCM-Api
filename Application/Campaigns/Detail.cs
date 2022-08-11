using Core.Campaigns;
using MediatR;
using Persistence.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Detail
    {
        public class Query : IRequest<Campaign>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Campaign>
        {
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }


            public async Task<Campaign> Handle(Query request, CancellationToken cancellationToken)
            {
                var campaign = await _context.Campaigns.FindAsync(request.Id);

                return campaign;
            }
        }
    }
}
