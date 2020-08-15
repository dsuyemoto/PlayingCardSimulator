using static Dealer.TableBase;

namespace Dealer
{
    public class TexasHoldemPlayerStreet : StreetBase
    {
        TexasHoldemBase _texasHoldemBase;

        public override int NumberOfCards { get; set; }
        public override bool IsHidden { get; set; }
        public override StreetName Name { get; set; }

        public TexasHoldemPlayerStreet(TexasHoldemBase texasHoldemBase, int numberOfCards, bool isHidden, StreetName name) : base(numberOfCards, isHidden, name)
        {
            _texasHoldemBase = texasHoldemBase;
        }

        public override void DealCards()
        {
            _texasHoldemBase.FixDealerButton();
            _texasHoldemBase.SetBlinds();
            _texasHoldemBase.DealPlayerCards(this);
        }

        public override void StartBettingRound(int startingSeatNumber)
        {
            var bigBlindPlayer = _texasHoldemBase.GetBlindPlayer(Player.BlindName.Big);
            if (bigBlindPlayer != null)
                startingSeatNumber = bigBlindPlayer.SeatNumber;

            _texasHoldemBase.StartBettingRound(startingSeatNumber);
        }
    }
}
