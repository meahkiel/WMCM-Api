using System;

namespace Repositories.DTO
{
    public class ContactListDTO
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public string Gender { get; set; }
        public string FullName { get; set; }
       
        public string MobileNo { get; set; }
        public string PrimaryContact { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }
        public string GroupTag { get; set; }

    }
}
