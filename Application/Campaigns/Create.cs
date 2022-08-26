using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

         

            public async Task<Result<CampaignDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var campaign = _mapper.Map<Campaign>(request.Campaign);
                campaign.Id = Guid.NewGuid();
                _context.Campaigns.Add(campaign);

                try
                {
                    var result = await _context.SaveChangesAsync() > 0;
                    if (!result)
                    {
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
