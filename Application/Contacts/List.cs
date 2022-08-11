using Application.Core;
using Application.DTO;
using Core.Contacts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public class List
    {
        public class Query : IRequest<Result<List<ContactListDTO>>>
        {

        }

        public class QueryHandler : IRequestHandler<Query, Result<List<ContactListDTO>>>
        {
            private readonly DataContext _context;

            public QueryHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ContactListDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var contacts = await _context.Contacts.Select(s => new ContactListDTO { 
                    Id = s.Id,
                    Title = s.ToTitleString(),
                    FullName = $"{s.FirstName} {s.MiddleName.Substring(0,1).ToUpper()} {s.LastName}",
                    EmailAddress = s.EmailAddress,
                    Gender = s.ToGenderString(),
                    MobileNo = s.MobileNo,
                    PrimaryContact = string.IsNullOrEmpty(s.PrimaryContact) ? s.MobileNo : s.PrimaryContact,
                    Location = s.Location,
                    GroupTag = s.GroupTag,
                }).ToListAsync(cancellationToken: cancellationToken);
                
                return Result<List<ContactListDTO>>.Success(contacts);
            }
        }
    }
}
