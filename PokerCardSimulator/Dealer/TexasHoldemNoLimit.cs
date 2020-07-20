using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dealer
{
    public class TexasHoldemNoLimit : TexasHoldemBase
    {
        public override int DealerButtonSeatNumber { get; set; }
        public override int BigBlindSeatNumber { get; set; }
        public override int SmallBlindSeatNumber { get; set; }
        public override int StartDealingSeatNumber { get ; set ; }
        public override List<Card> Community { get; set; } = new List<Card>();
        public override int TableId { get; set ; }
        public override int Seats { get; set; }
        public override decimal Pot { get; set; }
        public override List<StreetBase> Streets { get; set; } = new List<StreetBase>();
        public override int PlayerTimeout { get; set; }
        public override Task RunningGame { get; set; }
        public override Deck Deck { get; set; }
        public override decimal LastBet { get; set; }
        public override CancellationTokenSource GameCancellationSource { get; set; }
        public override decimal SmallBlind { get; set; }
        public override decimal BigBlind { get; set; }
        public override List<Player> Players { get; } = new List<Player>();

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
            PlayerTimeout = playerTimeout;
            InitializeStreets();
        }

        protected override TableViewBase GetTableView(int playerId)
        {
            return new TexasHoldemView(this, playerId);
        }
    }
}
