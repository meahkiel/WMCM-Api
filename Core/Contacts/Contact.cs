using Core.Base;
using Core.Enum;
using System;

namespace Core.Contacts
{
    public class Contact : BaseEntity
    {

        public static Salutation ToSalutation(string salutation)
        {
            return salutation.ToLower() switch
            {
                "mr" => Salutation.Mr,
                "ms" => Salutation.Ms,
                "mrs" => Salutation.Mrs,
                _ => Salutation.Mr,
            };
        }

        public static GenderEnum ToGender(string gender)
        {
            return gender.ToLower() switch
            {
                "male" => GenderEnum.Male,
                "female" => GenderEnum.Female,
                _ => GenderEnum.Male
            };
        }

        public static Contact Create(string title,string firstName,string middleName, 
            string lastName,string gender,string mobileNo,string emailAddress,
            string primaryContact, string location,string groupTag)
        {

            return new Contact
            {
                Id = Guid.NewGuid(),
                Title = ToSalutation(title),
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = ToGender(gender),
                MobileNo = mobileNo,
                EmailAddress = emailAddress,
                PrimaryContact = primaryContact == null ? mobileNo : primaryContact,
                Location = location,
                GroupTag = groupTag
            };
        }

        public string ToTitleString()
        {
            return Title switch
            {
                Salutation.Mr => "mr",
                Salutation.Ms => "ms",
                Salutation.Mrs => "mrs",
                _ => "mr"
            };
        }

        public string ToGenderString()
        {
            return Gender switch
            {
                GenderEnum.Female => "female",
                _ => "male"
            };
        }

        public string ToFullName()
        {
            var middleName = string.IsNullOrEmpty(MiddleName) ? string.Empty :MiddleName.Substring(0, 1);
            return $"{FirstName} {middleName} {LastName}";
        }

        public Salutation Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }

        public string Location { get; set; }

        public string GroupTag { get; set; }

        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }

        public string PrimaryContact { get; set; }

    }
}
