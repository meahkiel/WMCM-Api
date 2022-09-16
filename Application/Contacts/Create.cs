using Application.Core;
using Application.DTO;
using Core.Contacts;
using MediatR;
using Repositories.Unit;
using System;
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
            private readonly UnitWrapper _context;
            
            public CommandHandler(UnitWrapper context)
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
                
                try
                {
                     _context.Customers.Add(contact);
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
