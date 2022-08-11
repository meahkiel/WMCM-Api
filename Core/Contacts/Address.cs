namespace Core
{
    public class Address
    {

        public Address()
        {
            
        }
        public Address(string no,string roomno, string floor,string street,string city,string zone) 
            : base()
        {
            this.No = no;
            this.RoomNo = roomno;
            this.Floor = floor;
            this.Street = street;
            this.City = city;
            this.Zone = zone;
        }
        public string No { get; private set; }

        public string RoomNo { get; private set; }

        public string Floor { get;  private set; }

        public string Street { get; private set; }

        public string City { get; private set; }

        public string Zone { get; private set; }
    }
}