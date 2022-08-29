using Application.Core;
using Application.DTO;
using Core.Contacts;
using Infrastructure.Repositories.Customers;
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
            private readonly ICustomerRepo _repo;
            
            public CommandHandler(ICustomerRepo repo)
            {
                _repo = repo;
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
                    await _repo.Create(contact);
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
