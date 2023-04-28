using Application.SeedWorks;
using Core.Channels;

namespace Application.Channels
{
    public class GetById
    {
        public class Query : IRequest<Result<ChannelSetting>>
        {
            public string Id { get; set; }
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
                    var channel = await _context.Channels.FindByIdAsync(request.Id);

                    return Result<ChannelSetting>.Success(channel);
                }
                catch (Exception e)
                {
                    return Result<ChannelSetting>.Failure(e.Message);
                }
            }
        }
    }
}
