using Application.Core;
using Application.DTOs;
using AutoMapper;
using Core.Enum;
using Core.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
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
            private readonly IUserAccessorService _userAccessorService;
            public CommandHandler(UnitWrapper context, IMapper mapper,
                IUserAccessorService userAccessorService)
            {
                _context = context;
                _mapper = mapper;
                _userAccessorService = userAccessorService; 

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    var subTasks = new List<SubTask>();

                    var marketingTask = await 
                        _context.Marketings.MarketingTasks
                                .Include(s => s.SubTasks)
                                .Where(m => m.Id == request.MarketingTask.Id)
                                .SingleOrDefaultAsync();

                    if (marketingTask == null) {
                        throw new Exception("Error Task");
                    }

                    var roles = await _userAccessorService.GetUserRole();
                    var currentUser = _userAccessorService.GetUsername();

                    foreach (var task in request.MarketingTask.SubTasks)
                    {   
                        if (marketingTask.SubTasks.Count == 0 || string.IsNullOrEmpty(task.Id))
                        {
                           
                            //add it
                            var subTask = new SubTask
                            {
                                Id = Guid.NewGuid(),
                                AssignedTo = (roles.FirstOrDefault() == RoleEnum.Manager.ToString().ToLower()) ? task.AssignedTo : currentUser,
                                AssignedBy = currentUser,
                                Task = task.Task,
                                Status = StatusEnum.Todo.ToString(),
                                MarketingTask = marketingTask
                            };
                            _context.Marketings.AddSubTask(subTask);
                        }
                        else
                        {   
                            var subTask = marketingTask.SubTasks
                                                .Where(s => s.Id == Guid.Parse(task.Id))
                                                .FirstOrDefault();
                            if (task.MarkDelete)
                            {
                                if(subTask.AssignedBy == currentUser
                                    || subTask.AssignedTo == currentUser)
                                {
                                    _context.Marketings.DeleteUpdateSubTask(subTask);
                                }
                            }
                            else
                            {
                                if(roles.FirstOrDefault() == RoleEnum.Manager.ToString().ToLower())
                                {
                                    subTask.AssignedTo = task.AssignedTo;
                                }

                                if(subTask.AssignedBy == currentUser) {
                                    subTask.Task = task.Task;
                                }

                                subTask.Status = task.Status;
                                _context.Marketings.UpdateSubTask(subTask);
                            }

                        }
                    }
                    
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
