using Application.Core;
using Application.DTO;
using AutoMapper;
using Infrastructure.Repositories.Customers;
using MediatR;
using Persistence.Context;
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
           
            private readonly ICustomerRepo _repo;
            private readonly IMapper _mapper;

            public QueryHandler(ICustomerRepo repo, IMapper mapper)
            {
               
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<Result<ContactFormDTO>> Handle(Query request, CancellationToken cancellationToken) {
                
                var contact = await _repo.GetSingle(request.Id);

                return Result<ContactFormDTO>
                    .Success(_mapper.Map<ContactFormDTO>(contact));
            }

            
        }
    }
}
