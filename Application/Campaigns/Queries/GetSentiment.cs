using Application.Interface;
using Application.SeedWorks;

namespace Application.Campaigns.Queries;

public static class GetSentiment
{
    public record Query(string Word) : IRequest<Result<object>>
    {
        public string? FileName { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, Result<object>>
    {
        private readonly ISentimentService _service;

        public QueryHandler(ISentimentService service)
        {
            _service = service;
        }
        public async Task<Result<object>> Handle(Query request, CancellationToken cancellationToken)
        {
           try
            {

                return  Result<object>.Success( await _service.GetSentiment(request.Word.ToLower(),request.FileName));
            }catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }
    }
}
