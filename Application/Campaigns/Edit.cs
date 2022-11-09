using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Edit
    {

        public class Command : IRequest<Result<Unit>>
        {
            public CampaignDTO Campaign { get; set; }
        }

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
                    var campaign = _mapper.Map<Campaign>(request.Campaign);
                    _context.CampaignRepo.Update(campaign);
                    await _context.SaveChangesAsync();

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {

                    return Result<Unit>.Failure(e.Message);
                }
               
            }
        }

    }
}
