using System.Collections.Generic;

namespace Dealer
{
    public class Hand
    {
        public int Id { get; set; }
        public List<Card> Cards { get; set; }
        public int Score { get; set; }

        public Hand(int id, List<Card> cards)
        {
            Id = id;
            Cards = cards;
        }
    }
}
