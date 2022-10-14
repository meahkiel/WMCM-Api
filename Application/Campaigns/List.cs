using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Campaigns
{
    public class List
    {
        public class Query : IRequest<Result<CampaignListDTO>>
        {

        }


        public class QueryHandler : IRequestHandler<Query, Result<CampaignListDTO>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public QueryHandler(UnitWrapper context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<CampaignListDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var data = await _context.CampaignRepo.GetActiveCampaigns();
                    CampaignListDTO list = new CampaignListDTO();

                    if(data != null)
                    {
                        foreach(var item in data) {
                            var dto = _mapper.Map<CampaignDTO>(item);
                            
                            list.Campaigns.Add(dto);

                            list.TotalSMSActivities = item.Activities
                                                            .Where(a => a.Type == "sms")
                                                            .Count();

                            list.TotalEmailActivities = item.Activities
                                                            .Where(a => a.Type == "email")
                                                            .Count();

                            list.TotalEcommerce = item.Activities
                                                        .Where(a => a.Type == "webmail")
                                                        .Count();

                            list.TotalSocialPost = item.Activities
                                                         .Where(a => a.Type == "social")
                                                        .Count();

                        }
                    }

                    return Result<CampaignListDTO>.Success(list);
                }
                catch (Exception ex)
                {
                    return Result<CampaignListDTO>.Failure(ex.Message);
                }
                

            }
        }
    }
}
