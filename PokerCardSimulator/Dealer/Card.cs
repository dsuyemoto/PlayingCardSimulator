namespace Dealer
{
    public class Card
    {
        public enum Rank
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
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
        public bool IsHidden { get; set; }

        public Card()
        {

        }

        public override string ToString()
        {
            return RankValue.ToString() + SuitValue.ToString();
        }
    }
}
