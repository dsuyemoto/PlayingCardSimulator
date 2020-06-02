using System;
using System.Net.Security;

namespace CardDeck
{
    public class Card
    {
        public enum Rank
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        } 
        public enum Suit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }
        public Rank RankValue { get; set; }
        public Suit SuitValue { get; set; }

        public Card()
        {

        }
    }
}
