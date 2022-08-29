using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Contacts;
using Infrastructure.Repositories.Customers;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public class Update
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContactFormDTO ContactForm { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command,Result<Unit>>
        {
            private readonly ICustomerRepo _repo;
            private readonly IMapper _mapper;

            public CommandHandler(ICustomerRepo repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                try
                {

                    var contact = _mapper.Map<Contact>(request.ContactForm);
                    if(!await _repo.Update(contact))
                    {
                        throw new Exception("Contact cannot update");
                    }

                    return Result<Unit>.Success(Unit.Value);

                }
                catch(Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }
    }
}
