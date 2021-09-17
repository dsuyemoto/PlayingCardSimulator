using static Dealer.TableBase;

namespace Dealer
{
    public abstract class StreetBase
    {
        public abstract int NumberOfCards { get; set; }
        public abstract bool IsHidden { get; set; }
        public abstract StreetName Name { get; set; }

        public StreetBase(int numberOfCards, bool isHidden, StreetName name)
        {
            NumberOfCards = numberOfCards;
            IsHidden = isHidden;
            Name = name;
        }

        public abstract void DealCards();
        public abstract void StartBettingRound(int startingSeatNumber);
    }
}
