using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Create
    {
        public class Command : IRequest<Result<CampaignDTO>>
        {
            public CampaignDTO Campaign { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<CampaignDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public CommandHandler(UnitWrapper context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<CampaignDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var campaign = _mapper.Map<Campaign>(request.Campaign);
                campaign.Id = Guid.NewGuid();
                _context.CampaignRepo.Add(campaign);

                try
                {
                    
                    var result = await _context.SaveChangesAsync();

                    if (!result) {
                        throw new Exception("Failed to create campaign");
                    }
                    
                    return Result<CampaignDTO>.Success(_mapper.Map<CampaignDTO>(campaign));
                }
                catch(Exception ex)
                {
                    return Result<CampaignDTO>.Failure(ex.Message);
                }
            }
        }
    }
}
