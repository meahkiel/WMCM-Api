using Application.Core;
using Application.DTO;
using Core.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
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
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<ActivityInitial>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<string> groups = new List<string>();
                List<Template> templates = new List<Template>();

                if(request.Type == "sms")
                {
                    groups = await _context.Contacts
                                    .Select(c => c.GroupTag).Distinct().ToListAsync();
                    templates = await _context.Templates.Where(t => t.Type.ToLower() == "sms").ToListAsync();

                }

                return Result<ActivityInitial>.Success(new ActivityInitial(groups.ToArray(),templates));
            }
        }
    }
}
