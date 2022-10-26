using Application.Core;
using Core.Channels;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Channels
{
    public class GetByType
    {

        public class Query : IRequest<Result<ChannelSetting>>
        {
            public string Type { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<ChannelSetting>>
        {
            private readonly UnitWrapper _context;   
            public QueryHandler(UnitWrapper context)
            {
                _context = context;
            }
            public async Task<Result<ChannelSetting>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var channel = await _context.Channels.FindByTypeAsync(request.Type);
                    
                    return Result<ChannelSetting>.Success(channel);
                }
                catch(Exception e)
                {
                    return Result<ChannelSetting>.Failure(e.Message);
                }
            }
        }

    }
}
