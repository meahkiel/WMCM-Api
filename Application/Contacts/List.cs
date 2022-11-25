using Application.DTO;
using Application.SeedWorks;
using AutoMapper;
using MediatR;
using Repositories.Unit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public class List
    {
        public class Query : IRequest<Result<List<ContactListDTO>>>
        {

        }

        public class QueryHandler : IRequestHandler<Query, Result<List<ContactListDTO>>>
        {
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public QueryHandler(UnitWrapper context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ContactListDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var contacts = await _context.Customers.GetActives();
                
                return Result<List<ContactListDTO>>.Success(_mapper.Map<List<ContactListDTO>>(contacts));
            }
        }
    }
}
