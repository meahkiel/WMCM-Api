using Application.Core;
using Application.DTOs;
using AutoMapper;
using Core.Tasks;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MarketingTasks
{
    public class LoadSubTask
    {
        public record Query(string TaskId = "") : IRequest<Result<MarketingTaskDTO>>;

        public class QueryHandler : IRequestHandler<Query, Result<MarketingTaskDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public QueryHandler(UnitWrapper context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<MarketingTaskDTO>> Handle(Query request, CancellationToken cancellationToken)
            {

                var task = await _context.MarketingRepo.GetSubTask(Guid.Parse(request.TaskId));

                return Result<MarketingTaskDTO>.Success(_mapper.Map<MarketingTaskDTO>(task));
            }
        }
    }
}
