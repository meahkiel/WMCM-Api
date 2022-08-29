using Core.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contacts
{
    public class ContactSerialize
    {
        private readonly List<Contact> _contacts;
        private string _additional;

        public ContactSerialize(List<Contact> contacts, string additional)
        {
            _contacts = contacts;
            _additional = additional;
        }

        public List<string> ExtractSMS()
        {
            List<string> list = new List<string>();
            foreach (Contact contact in _contacts)
            {
                list.Add(contact.MobileNo);
            }

            if (!String.IsNullOrEmpty(_additional))
            {
                var strAdditional = _additional.Split(',');
                list.AddRange(strAdditional);
            }

            return list;
        }
    }

       
}
