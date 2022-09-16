using Application.Core;
using Application.DTO;
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

namespace Application.Activities
{
    public class Initialize
    {
        public class Query: IRequest<Result<ActivityInitial>>
        {

            public string Type { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<ActivityInitial>>
        {
            private readonly UnitWrapper _context;

            public QueryHandler(UnitWrapper context)
            {
                _context = context;
            }
            public async Task<Result<ActivityInitial>> Handle(Query request, CancellationToken cancellationToken)
            {
               

                var groups = await _context.Customers.GetGroupContact();
                var templates = await _context.TemplateRepo.GetAllTemplatesAsync(request.Type.ToLower());

                return Result<ActivityInitial>.Success(new ActivityInitial(groups.ToArray(),templates.ToList()));
            }
        }
    }
}
