﻿using Application.SeedWorks;
using Core.Channels;

namespace Application.Channels
{
    public class All
    {

        public class Query : IRequest<Result<IEnumerable<ChannelSetting>>>
        {
           
        }

        public class QueryHandler : IRequestHandler<Query, Result<IEnumerable<ChannelSetting>>>
        {
            private readonly UnitWrapper _context;

            public QueryHandler(UnitWrapper context)
            {
                _context = context;
            }

            public async Task<Result<IEnumerable<ChannelSetting>>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    return Result<IEnumerable<ChannelSetting>>
                            .Success(await _context.Channels.GetChannels());
                }
                catch(Exception ex)
                {
                    return Result<IEnumerable<ChannelSetting>>.Failure(ex.Message);
                }
            }
        }

    }
}
