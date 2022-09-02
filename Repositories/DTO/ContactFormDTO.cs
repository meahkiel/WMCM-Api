using Core.Enum;

namespace Repositories.DTO
{
    public class ContactFormDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }

        public string Location { get; set; }

        public string GroupTag { get; set; }
        public string Type { get; set; }

        public string PrimaryContact { get; set; }


        public Salutation ToSalutation()
        {
            
            return Title.ToLower() switch
            {
                "mr" => Salutation.Mr,
                "ms" => Salutation.Ms,
                "mrs" => Salutation.Mrs,
                _ => Salutation.Mr,
            };
        }

        public  GenderEnum ToGender()
        {
            
            return Gender.ToLower() switch
            {
                "male" => GenderEnum.Male,
                "female" => GenderEnum.Female,
                _ => GenderEnum.Male
            };
        }

    }
}
