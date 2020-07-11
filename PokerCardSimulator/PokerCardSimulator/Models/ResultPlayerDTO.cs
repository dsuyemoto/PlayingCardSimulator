using PokerCardSimulator.Models;

namespace PokerCardSimulator.Controllers
{
    public class ResultPlayerDTO
    {
        public PlayerDTO Player { get; set; }
        public ResultErrorDTO Error { get; set; }

        public ResultPlayerDTO()
        {
            
        }
    }
}