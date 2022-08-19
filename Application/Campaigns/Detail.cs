using Application.Core;
using Core.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Detail
    {
        public class Query : IRequest<Result<Campaign>>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Campaign>>
        {
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }


            public async Task<Result<Campaign>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var campaign = await _context.Campaigns
                        .Include(c => c.Activities)
                        .Where(c => c.Id == request.Id).SingleAsync();
                    
                    foreach(var activity in campaign.Activities)
                    {
                        activity.Campaign = null;
                    }

                    if (campaign == null) throw new Exception("Campaign Error");
                    return Result<Campaign>.Success(campaign);
                }catch (Exception ex)
                {
                    return Result<Campaign>.Failure(ex.Message);
                }
            }
        }
    }
}
