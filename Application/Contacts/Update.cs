using Application.Core;
using Application.DTO;
using AutoMapper;
using Core.Contacts;
using MediatR;
using Repositories.Unit;
using System;
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
            private readonly UnitWrapper _context;
            private readonly IMapper _mapper;

            public CommandHandler(UnitWrapper context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                try
                {

                    var contact = _mapper.Map<Contact>(request.ContactForm);
                    _context.CustomerRepo.Update(contact);
                    if (!await _context.SaveChangesAsync())
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
