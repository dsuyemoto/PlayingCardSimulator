using System.Collections.Generic;
using static Dealer.Deck;

namespace Dealer
{
    public class Hand
    {
        public int PlayerId { get; set; }
        public List<Card> Cards { get; set; }
        public int Score { get; set; }
        public Ranking Ranking { get; set; }

        public Hand(int playerId, List<Card> cards)
        {
            PlayerId = playerId;
            Cards = cards;
        }
    }
}
