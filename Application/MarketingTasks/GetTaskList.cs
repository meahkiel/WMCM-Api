using Application.Core;
using Application.DTOs;
using AutoMapper;
using Core.Enum;
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
    public class GetTaskList
    {
        public record Query(string UserName = "") : IRequest<Result<IEnumerable<MarketingTaskDTO>>>;

        public class IQueryHandler : IRequestHandler<Query,Result<IEnumerable<MarketingTaskDTO>>>
        {
            private readonly UnitWrapper _context;
            private readonly IUserAccessorService _userAccessor;
            private readonly IMapper _mapper;

            public IQueryHandler(UnitWrapper context,IUserAccessorService userAccessor,IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

           
            public async Task<Result<IEnumerable<MarketingTaskDTO>>> Handle(Query request, CancellationToken cancellationToken) 
            {
                try
                {
                    var userRoles = await _userAccessor.GetUserRole();
                    bool isHigherUser = false;
                    foreach (var role in userRoles)
                    {
                        if (role == RoleEnum.Manager.ToString().ToLower())
                        {
                            isHigherUser = true;
                        }
                    }
                        var tasks = await _context.Marketings.GetListByUser(_userAccessor.GetUsername(),isHigherUser);
                    var taskDtos = _mapper.Map<IEnumerable<MarketingTaskDTO>>(tasks);
                  
                    return Result<IEnumerable<MarketingTaskDTO>>.Success(taskDtos);
                }
                catch (Exception ex)
                {
                    return Result<IEnumerable<MarketingTaskDTO>>.Failure(ex.Message);
                }

                
            }
                
        }


    }
}
