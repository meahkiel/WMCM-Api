using Core.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Customers
{
    public interface ICustomerRepo
    {
        Task<bool> Create(Contact contact);
        Task<Contact> GetSingle(Guid id);
        Task<IEnumerable<Contact>> GetActives();
        Task<IEnumerable<Contact>> GetContactByGroup(string groupTag);
        Task<bool> Inactivate(Guid id);
        Task<bool> Update(Contact contact);
       

        
    }
}
