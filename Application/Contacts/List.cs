using Application.Core;
using Application.DTO;
using AutoMapper;
using Infrastructure.Repositories.Customers;
using MediatR;
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
            private readonly ICustomerRepo _repo;
            private readonly IMapper _mapper;

            public QueryHandler(ICustomerRepo repo,IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<Result<List<ContactListDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                var contacts = await _repo.GetActives();
                
                return Result<List<ContactListDTO>>.Success(_mapper.Map<List<ContactListDTO>>(contacts));
            }
        }
    }
}
