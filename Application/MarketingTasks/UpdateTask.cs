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
    public class UpdateTask
    {
        public record Command(MarketingTaskDTO MarketingTask = null) : IRequest<Result<Unit>>;

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;
            public CommandHandler(UnitWrapper context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {

                    var subTasks = _mapper.Map<IList<SubTask>>(request.MarketingTask.SubTasks.ToList());

                    await _context.Marketings.CreateUpdateSubTask(request.MarketingTask.Id, subTasks); 
                    var result = await _context.SaveChangesAsync();

                    if (!result)
                        throw new Exception("Unable to Add or Update SubTask");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }

    }
}
