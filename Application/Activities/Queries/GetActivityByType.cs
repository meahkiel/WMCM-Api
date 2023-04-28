using Application.DTO;
using Application.SeedWorks;

namespace Application.Activities.Queries;

public static class GetActivityByType
{
    public record Query() : IRequest<Result<ActivityEntryDTO>>;

    public class QueryHandler : IRequestHandler<Query, Result<ActivityEntryDTO>>
    {
        private readonly UnitWrapper _context;
        public QueryHandler(UnitWrapper context)
        {
            _context = context;
        }
        public async Task<Result<ActivityEntryDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.CampaignRepo.GetActivityByType("web");
            if (activity == null)
                return Result<ActivityEntryDTO>.Failure("Activity not found");
            
            var activityEntry = new ActivityEntryDTO
            {
                Id = activity.Id.ToString(),
                Title = activity.Title,
                Body = activity.Body,
                CoverImage = activity.CoverImage
            };

            return Result<ActivityEntryDTO>.Success(activityEntry);
        }
    }

}
