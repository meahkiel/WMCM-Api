using Application.Core;
using Application.DTO;
using Core.Contacts;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContactFormDTO ContactForm { get; set; }

        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public CommandHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, 
                CancellationToken cancellationToken)
            {

                var contact = Contact.Create(
                    request.ContactForm.Title,
                    request.ContactForm.FirstName,
                    request.ContactForm.MiddleName,
                    request.ContactForm.LastName,
                    request.ContactForm.Gender,
                    request.ContactForm.MobileNo,
                    request.ContactForm.EmailAddress,
                    request.ContactForm.PrimaryContact,
                    request.ContactForm.Location,
                    request.ContactForm.GroupTag);

                _context.Contacts.Add(contact);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
