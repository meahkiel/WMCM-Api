using Application.Core;
using Application.DTOs;
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
    public class List
    {
        public record Query(string UserName = "") : IRequest<Result<IEnumerable<MarketingTaskDTO>>>;

        public class IQueryHandler : IRequestHandler<Query,Result<IEnumerable<MarketingTaskDTO>>>
        {
            private readonly UnitWrapper _context;
            private readonly IUserAccessorService _userAccessor;

            public IQueryHandler(UnitWrapper context,IUserAccessorService userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

           
            public async Task<Result<IEnumerable<MarketingTaskDTO>>> Handle(Query request, CancellationToken cancellationToken) 
            { 

                var tasks = await _context.MarketingRepo.GetListByUser(_userAccessor.GetUsername());
                var taskDtos = new List<MarketingTaskDTO>();
                foreach (var task in tasks) {
                    taskDtos.Add(new MarketingTaskDTO
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Close = task.Close,
                        UserName = task.UserName,
                        TotalSubTaskCount = task.SubTasks.Count(w => w.Status != StatusEnum.Done.ToString()),
                    });
                }
                return Result<IEnumerable<MarketingTaskDTO>>.Success(taskDtos);
            }
                
        }


    }
}
