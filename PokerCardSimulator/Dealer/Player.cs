using System.Collections.Generic;

namespace Dealer
{
    public class Player
    {
        public int Id { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public double Chips { get; set; }

        public Player(int id, double chips)
        {
            Id = id;
            Chips = chips;
        }
    }
}
