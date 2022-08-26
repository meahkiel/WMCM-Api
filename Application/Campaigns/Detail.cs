using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class Detail
    {
        public class Query : IRequest<Result<CampaignDTO>>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<CampaignDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }


            public async Task<Result<CampaignDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var campaign = await _context.Campaigns
                        .Include(c => c.Activities)
                        .Where(c => c.Id == request.Id)
                        .Select(c => new CampaignDTO
                        {
                            Id = c.Id,
                            DateFrom = c.DateFrom,
                            DateTo = c.DateTo,
                            Description = c.Description,
                            Title = c.Title,
                            Activities = c.Activities
                                            .Select(a => new ActivityCampaignDTO { Title = a.Title,Type = a.Type, DateCreated = a.DispatchDate,Status = a.Status } )
                        }).FirstOrDefaultAsync();
                    
                    
                    if (campaign == null) throw new Exception("Campaign Error");
                    return Result<CampaignDTO>.Success(campaign);
                }catch (Exception ex)
                {
                    return Result<CampaignDTO>.Failure(ex.Message);
                }
            }
        }
    }
}
