using Application.DTO;
using Application.SeedWorks;
using AutoMapper;
using MediatR;
using Repositories.Unit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public  class Find
    {
        public class Query : IRequest<Result<ContactFormDTO>>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<ContactFormDTO>>
        {
           
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public QueryHandler(UnitWrapper context, IMapper mapper)
            {

                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ContactFormDTO>> Handle(Query request, CancellationToken cancellationToken) {
                
                var contact = await _context.Customers.GetSingle(request.Id);

                return Result<ContactFormDTO>
                    .Success(_mapper.Map<ContactFormDTO>(contact));
            }

            
        }
    }
}
