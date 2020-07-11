using static Dealer.TableBase;

namespace Dealer
{
    public class CommunityStreet : StreetBase
    {
        TexasHoldemBase _texasHoldemBase;

        public override int NumberOfCards { get; set; }
        public override bool IsHidden { get; set; }
        public override StreetName Name { get; set; }

        public CommunityStreet(TexasHoldemBase texasHoldemBase, int numberOfCards, bool isHidden, StreetName name) : base(numberOfCards, isHidden, name)
        {
            _texasHoldemBase = texasHoldemBase;
        }

        public override void DealCards()
        {
            _texasHoldemBase.DealCommunityCards(this);
        }
    }
}
