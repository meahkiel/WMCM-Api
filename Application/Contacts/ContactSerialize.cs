using Core.Contacts;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        public string Serialize(string type)
        {
            List<string> list = new List<string>();
            foreach (Contact contact in _contacts)
            {
                if (type == "mobile")
                    list.Add(contact.MobileNo);
                else if (type == "email")
                    list.Add(contact.EmailAddress);
                else
                    continue;
            }

            if (!String.IsNullOrEmpty(_additional))
            {
                var strAdditional = _additional.Split(',');
                list.AddRange(strAdditional);
            }

            return string.Join(",", list.ToArray());
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


       
        public List<string> ExtractEmail()
        {
            List<string> list = new List<string>();
            foreach (Contact contact in _contacts)
            {
                list.Add(contact.EmailAddress);
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
