using Application.DTOs;
using Application.Interface;
using Application.SeedWorks;
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
            private readonly IPublisher _publisher;
            private IList<NotifyTaskEvent> _notifyTaskEvents;

            public CommandHandler(UnitWrapper context, IMapper mapper,
                IUserAccessorService userAccessorService,IPublisher publisher)
            {
                _context = context;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
                _publisher = publisher;
                
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
                    _notifyTaskEvents = new List<NotifyTaskEvent>();
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
                                Status = TaskStatusEnum.Todo.ToString(),
                                MarketingTask = marketingTask
                            };
                            _context.Marketings.AddSubTask(subTask);
                            
                            //raised an event
                            _notifyTaskEvents.Add(new NotifyTaskEvent
                            {
                                Description = $"New Task Has been Created {subTask.Task}",
                                Module = "Marketing Task",
                                AssignedTo = subTask.AssignedTo
                            });
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
                                if(subTask.Status != task.Status)
                                {
                                    subTask.Status = task.Status;
                                    
                                    //raised an event
                                    _notifyTaskEvents.Add(new NotifyTaskEvent
                                    {
                                        Description = $"Task status has been updated by {subTask.AssignedTo}",
                                        Module = "Marketing Task",
                                        AssignedTo = subTask.AssignedBy
                                    });

                                }
                                
                                _context.Marketings.UpdateSubTask(subTask);
                            }

                        }
                    }
                    
                    var result = await _context.SaveChangesAsync();

                    if (!result)
                        throw new Exception("Unable to Add or Update SubTask");


                    //published the event
                    if (_notifyTaskEvents.Count > 0)
                    {
                        foreach (var notificationEvent in _notifyTaskEvents)
                        {
                            await _publisher.Publish(notificationEvent);
                        }
                    }

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
