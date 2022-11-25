using Application.DTOs;
using Application.Interface;
using Application.SeedWorks;
using AutoMapper;
using Core.Enum;
using Core.Tasks;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MarketingTasks
{
    public class CreateTask
    {
        public record Command(MarketingTaskDTO MarketingTask = null)     : IRequest<Result<Unit>>;

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;
            

            public CommandHandler(UnitWrapper context,
                IMapper mapper,
                IUserAccessorService userAccessorService)
            {
                _context = context;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
               
            }


            public async Task<Result<Unit>> Handle(Command request, 
                CancellationToken cancellationToken)
            {
                try
                {
                    IList<string> userRoles = await _userAccessorService.GetUserRole();

                    if (!userRoles.Any(u => u == RoleEnum.Manager.ToString().ToLower()))
                        throw new Exception("User not allowed to create");

                    var marketing = _mapper.Map<MarketingTask>(request.MarketingTask);

                    //create a key
                    marketing.Id = Guid.NewGuid();
                    marketing.UserName = _userAccessorService.GetUsername();
                    _context.Marketings.Add(marketing);

                    var result = await _context
                                        .SaveChangesAsync();
                    
                    if (!result)
                        throw new Exception("Unable to save");

                    return Result<Unit>.Success(Unit.Value);
                }
                catch(Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }
    }
}
