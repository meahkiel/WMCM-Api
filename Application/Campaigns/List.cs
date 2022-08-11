using Core.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class List
    {
        public class Query : IRequest<List<Campaign>>
        {

        }


        public class QueryHandler : IRequestHandler<Query, List<Campaign>>
        {
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Campaign>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Campaigns.ToListAsync();
            }
        }
    }
}
