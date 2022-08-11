using Application.Core;
using Application.DTO;
using AutoMapper;
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
            private readonly DataContext _context;
          
            private readonly IMapper _mapper;

            public QueryHandler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ContactFormDTO>> Handle(Query request, CancellationToken cancellationToken) {
                
                var contact = await _context.Contacts.FindAsync(request.Id);

                return Result<ContactFormDTO>
                    .Success(_mapper.Map<ContactFormDTO>(contact));
            }

            
        }
    }
}
