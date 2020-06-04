namespace Dealer
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
        public bool IsHidden { get; set; }

        public Card()
        {

        }
    }
}
