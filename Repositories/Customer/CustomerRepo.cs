using Core.Contacts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Customer
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DataContext _context;
        public CustomerRepo(DataContext context)
        {
            _context = context;
        }

        public void Add(Contact entity)
        {
            _context.Attach(entity).State = EntityState.Added;
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

        public async Task<IEnumerable<string>> GetGroupContact()
        {
            return await _context.Contacts
                    .Select(c => c.GroupTag).Distinct().ToListAsync();
        }

        public async Task<Contact> GetSingle(Guid id)
        {
            return await _context.Contacts.SingleAsync(c => c.Id == id);
        }

        public Task<bool> Inactivate(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Contact entity)
        {
            _context.Contacts.Remove(entity);
        }

        public void Update(Contact entity)
        {
            _context.Attach(entity).State = EntityState.Modified;
        }
    }
}
