using Core.Contacts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Customers
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DataContext _context;

        public CustomerRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(Contact contact)
        {
            _context.Attach(contact).State = EntityState.Added;
            return await _context.SaveChangesAsync() > 0 ? true : false;

        }

        public async Task<IEnumerable<Contact>> GetActives()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactByGroup(string groupTag)
        {
            return await _context.Contacts
                        .Where(c => c.GroupTag.Contains(groupTag))
                        .ToListAsync();
        }

        public async Task<Contact> GetSingle(Guid id)
        {
            return await _context.Contacts.SingleAsync(c => c.Id == id);
        }

        public Task<bool> Inactivate(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Contact contact)
        {
            _context.Attach(contact).State = EntityState.Modified;
            
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
