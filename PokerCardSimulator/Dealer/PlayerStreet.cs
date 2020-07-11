using static Dealer.TableBase;

namespace Dealer
{
    public class PlayerStreet : StreetBase
    {
        TableBase _tableBase;

        public override int NumberOfCards { get; set; }
        public override bool IsHidden { get; set; }
        public override StreetName Name { get; set; }

        public PlayerStreet(TableBase tableBase, int numberOfCards, bool isHidden, StreetName name) : base(numberOfCards, isHidden, name)
        {
            _tableBase = tableBase;
        }

        public override void DealCards()
        {
            _tableBase.DealPlayerCards(this);
        }
    }
}
