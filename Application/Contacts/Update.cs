using Application.Core;
using Application.DTO;
using AutoMapper;
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
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                try
                {
                    Guid guidId = Guid.Parse(request.ContactForm.Id);
                    var contact = await _context.Contacts.FindAsync(guidId);
                    if (contact == null) {
                        throw new Exception("Employee Not Found");
                    }

                    _mapper.Map(request.ContactForm, contact);

                    var result = await _context.SaveChangesAsync();
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
