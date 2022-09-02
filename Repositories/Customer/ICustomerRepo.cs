using Core.Contacts;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Customer
{
    public interface ICustomerRepo : IRepositoryBase<Contact>
    {
       
        Task<Contact> GetSingle(Guid id);
        Task<IEnumerable<Contact>> GetActives();
        Task<IEnumerable<Contact>> GetContactByGroup(string groupTag);
        Task<bool> Inactivate(Guid id);

        Task<IEnumerable<string>> GetGroupContact();
    }
}
