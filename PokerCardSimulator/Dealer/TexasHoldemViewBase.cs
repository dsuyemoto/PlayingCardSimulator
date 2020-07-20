using System.Collections.Generic;

namespace Dealer
{
    public abstract class TexasHoldemViewBase : TableViewBase
    {
        public override int TableId { get; }
        public override int Seats { get; }
        public override decimal Pot { get; }
        public override int PlayerTimeout { get; }

        public abstract List<Card> Community { get; }
        public abstract int DealerButtonSeatNumber { get; }

        public TexasHoldemViewBase(TableBase tableBase, int playerId) : base(tableBase, playerId)
        {

        }
    }
}
