using Application.DTOs;
using Application.Interface;
using Application.SeedWorks;
using AutoMapper;
using Core.Enum;
using Core.Tasks;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MarketingTasks
{
    public class GetTask
    {
        public record Query(string TaskId = "") : IRequest<Result<MarketingTaskDTO>>;

        public class QueryHandler : IRequestHandler<Query, Result<MarketingTaskDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;

            public QueryHandler(UnitWrapper context,IUserAccessorService userAccessor, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _userAccessorService = userAccessor;

            }
            public async Task<Result<MarketingTaskDTO>> Handle(Query request, CancellationToken cancellationToken)
            {

                var task = new MarketingTask();
                var userRoles = await _userAccessorService.GetUserRole();

                foreach (var role in userRoles) {
                    if (role == RoleEnum.Manager.ToString().ToLower()) {
                        task = await _context.Marketings.GetSubTask(Guid.Parse(request.TaskId));
                        break;
                    }
                    else
                    {
                        task = await _context.Marketings.GetSubTask(Guid.Parse(request.TaskId), 
                            _userAccessorService.GetUsername());
                        break;
                    }
                }

                return Result<MarketingTaskDTO>.Success(_mapper.Map<MarketingTaskDTO>(task));
            }
        }
    }
}
