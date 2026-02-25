namespace Dealer
{
    public class Card
    {
        public Card(Rank rank, Suit suit, bool isHidden = false)
        {
            RankValue = rank;
            SuitValue = suit;
            IsHidden = isHidden;
        }
        
        public enum Rank
        {
            AceLow = 1,
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
            Club,
            Diamond,
            Heart,
            Spade
        }
        public Rank RankValue { get; }
        public Suit SuitValue { get; }
        public bool IsHidden { get; set; }

        public override string ToString()
        {
            return RankValue.ToString() + SuitValue.ToString();
        }

        public bool MatchCard(Card card)
        {
            return (card.SuitValue == SuitValue && card.RankValue == RankValue);
        }
    }
}
