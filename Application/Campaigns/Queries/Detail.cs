using Application.DTO;
using Application.SeedWorks;

namespace Application.Campaigns.Queries
{
    public class Detail
    {
        public class Query : IRequest<Result<CampaignDTO>>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<CampaignDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public QueryHandler(UnitWrapper context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }


            public async Task<Result<CampaignDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {

                    var campaign = await _context.CampaignRepo.GetSingleCampaign(request.Id);
                    if (campaign == null) throw new Exception("Campaign Error");

                    return Result<CampaignDTO>.Success(_mapper.Map<CampaignDTO>(campaign));
                }
                catch (Exception ex)
                {
                    return Result<CampaignDTO>.Failure(ex.Message);
                }
            }
        }
    }
}
