using System.Collections.Generic;

namespace Dealer
{
    public class Seat
    {
        public int Number { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public Player Player { get; set; }

        public Seat(int seatNumber)
        {
            Number = seatNumber;
        }
    }
}
