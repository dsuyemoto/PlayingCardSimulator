using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dealer
{
    public class TexasHoldemNoLimit : TexasHoldemBase
    {
        public override int ActionSeatPosition { get; set; }
        public override int DealerButtonSeatNumber { get; set; }
        public override int BigBlindSeatNumber { get; set; }
        public override int SmallBlindSeatNumber { get; set; }
        public override int StartDealingSeatNumber { get ; set ; }
        public override List<Card> Community { get; set; } = new List<Card>();
        public override int TableId { get; set ; }
        public override int Seats { get; set; }
        public override double Pot { get; set; }
        public override StreetName Street { get; set; } = StreetName.PreFlop;
        public override List<StreetBase> Streets { get; set; } = new List<StreetBase>();
        public override int StreetCount { get; set; }
        public override int PlayerTimeout { get; set; }
        public override Task RunningGame { get; set; }
        public override Deck Deck { get; set; }
        public override double LastBet { get; set; }
        public override double MinBet { get; set; }
        public override CancellationTokenSource GameCancellationSource { get; set; }
        public override List<Player> Players { get; set; } = new List<Player>();

        public TexasHoldemNoLimit(
            int tableId,
            Deck deck,
            double smallBlind,
            double bigBlind,
            int seats = 9,
            int dealerButton = 1,
            int playerTimeout = 30)
        {
            TableId = tableId;
            Deck = deck;
            _smallBlind = smallBlind;
            _bigBlind = bigBlind;
            DealerButtonSeatNumber = dealerButton;
            StartDealingSeatNumber = dealerButton;
            ActionSeatPosition = dealerButton;
            Seats = seats;
            Street = StreetName.PreFlop;
            PlayerTimeout = playerTimeout;
        }
    }
}
