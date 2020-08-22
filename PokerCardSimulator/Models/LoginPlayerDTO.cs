namespace PokerCardSimulator.Models
{
    public class LoginPlayerDTO
    {
        public string Username { get; set; }
        public string PasswordBase64 { get; set; }

        public LoginPlayerDTO()
        {

        }
    }
}