namespace PokerCardSimulator.Models
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public PlayerDTO()
        {

        }

    }
}
