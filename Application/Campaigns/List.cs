using Application.Core;
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
        public class Query : IRequest<Result<List<Campaign>>>
        {

        }


        public class QueryHandler : IRequestHandler<Query, Result<List<Campaign>>>
        {
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Campaign>>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _context.Campaigns.ToListAsync();
                    return Result<List<Campaign>>.Success(result);
                }
                catch (Exception ex)
                {
                    return Result<List<Campaign>>.Failure(ex.Message);
                }
                

            }
        }
    }
}
