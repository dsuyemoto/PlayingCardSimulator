using System.Collections.Generic;

namespace CardDeck
{
    public class Seat
    {
        public int Number { get; set; }
        public List<Card> Cards { get; set; }
        public Player Player { get; set; }

        public Seat()
        {

        }
    }
}
