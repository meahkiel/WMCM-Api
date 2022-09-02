using Application.Core;
using Application.DTO;
using Core.Contacts;
using MediatR;
using Persistence.Context;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            public CommandHandler(UnitWrapper context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, 
                CancellationToken cancellationToken)
            {
                var entries = request.Entries.ToList();

                if (entries != null && entries.Count > 0) {

                    foreach (var contactForm in entries) {
                        var contactEntry = Contact.Create(
                           contactForm.Title,
                           contactForm.FirstName,
                           contactForm.MiddleName,
                           contactForm.LastName,
                           contactForm.Gender,
                           contactForm.MobileNo,
                           contactForm.EmailAddress,
                           contactForm.PrimaryContact,
                           contactForm.Location,
                           contactForm.GroupTag);

                           _context.CustomerRepo.Add(contactEntry);
                    }

                    await _context.SaveChangesAsync();
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
