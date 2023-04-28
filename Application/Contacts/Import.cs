

using Application.DTO;
using Application.SeedWorks;
using Core.Contacts;

namespace Application.Contacts
{
    public class Import
    {
        public class Command : IRequest<Result<Unit>>
        {   
            public List<ContactFormDTO> Entries { get; set; }
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

            public async Task<Result<Unit>> Handle(Command request, 
                CancellationToken cancellationToken)
            {
              
                if (request.Entries != null && request.Entries.Count > 0) {
                    
                    foreach (var contactForm in request.Entries) {
                        var entry = _mapper.Map<Contact>(contactForm);
                        entry.Title = contactForm.ToSalutation();
                        entry.Gender = contactForm.ToGender();
                        _context.Customers.Add(entry);
                    }

                    await _context.SaveChangesAsync();
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
