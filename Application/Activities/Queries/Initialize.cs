using Application.DTO;
using Application.SeedWorks;

namespace Application.Activities.Queries;

public static class Initialize
{
    public record Query(string Type) : IRequest<Result<ActivityInitial>>;

    public class QueryHandler : IRequestHandler<Query, Result<ActivityInitial>>
    {
        private readonly UnitWrapper _context;

        public QueryHandler(UnitWrapper context)
        {
            _context = context;
        }
        public async Task<Result<ActivityInitial>> Handle(Query request, CancellationToken cancellationToken)
        {
            var groups = await _context.Customers.GetGroupContact();
            var templates = await _context.TemplateRepo.GetAllTemplatesAsync(request.Type.ToLower());

            return Result<ActivityInitial>.Success(new ActivityInitial(groups.ToArray(), templates.ToList()));
        }
    }
}
