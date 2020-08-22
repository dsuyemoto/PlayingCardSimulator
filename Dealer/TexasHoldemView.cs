using System.Collections.Generic;

namespace Dealer
{
    public class TexasHoldemView : TableViewBase
    {
        public int DealerButtonSeatNumber { get; }
        public List<Card> Community { get; }
        public override int TableId { get; }
        public override int Seats { get; }
        public override decimal Pot { get; }
        public override double PlayerTimeoutMilliseconds { get; }

        public TexasHoldemView(TexasHoldemBase texasHoldemBase, int playerId) : base(texasHoldemBase, playerId)
        {
            DealerButtonSeatNumber = texasHoldemBase.DealerButtonSeatNumber;
            Community = texasHoldemBase.Community;
            TableId = texasHoldemBase.TableId;
            Seats = texasHoldemBase.Seats;
            Pot = texasHoldemBase.Pot;
            PlayerTimeoutMilliseconds = texasHoldemBase.PlayerTimeoutMilliseconds;
        }
    }
}
