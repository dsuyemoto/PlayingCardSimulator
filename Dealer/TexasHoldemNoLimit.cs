using System.Collections.Generic;

namespace Dealer
{
    public class TexasHoldemNoLimit : TexasHoldemBase
    {
        public override int DealerButtonSeatNumber { get; set; }
        public override int StartDealingSeatNumber { get ; set ; }
        public override List<Card> Community { get; set; } = new List<Card>();
        public override int TableId { get; set ; }
        public override int Seats { get; set; }
        public override decimal Pot { get; set; }
        public override double PlayerTimeoutMilliseconds { get; set; }
        public override Deck Deck { get; set; }
        public override decimal LastBet { get; set; }
        public override decimal SmallBlind { get; set; }
        public override decimal BigBlind { get; set; }
        public override bool AutoStartEnabled { get; set; }

        public TexasHoldemNoLimit(
            int tableId,
            Deck deck,
            decimal smallBlind,
            decimal bigBlind,
            int seats = 9,
            int dealerButton = 1,
            int playerTimeout = 30)
        {
            TableId = tableId;
            Deck = deck;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            DealerButtonSeatNumber = dealerButton;
            StartDealingSeatNumber = dealerButton;
            Seats = seats;
            PlayerTimeoutMilliseconds = playerTimeout;
        }

        protected override TableViewBase GetTableView(int playerId)
        {
            return new TexasHoldemView(this, playerId);
        }
    }
}
