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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContactFormDTO ContactForm { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {

            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(DataContext context, IMapper mapper)
            {
                
                _context = context;
                _mapper = mapper;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken) {
                var contact = await _context.Contacts
                                        .FindAsync(request.ContactForm.Id, cancellationToken);

                if(contact == null) {
                    throw new Exception("Contact not found!!!");
                }

                _mapper.Map(request.ContactForm, contact);

                var result = await _context.SaveChangesAsync() > 0;

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
