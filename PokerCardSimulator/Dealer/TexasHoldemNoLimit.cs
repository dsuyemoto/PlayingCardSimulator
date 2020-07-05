using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dealer
{
    public class TexasHoldemNoLimit : TexasHoldemBase
    {
        public override int[] Blinds { get; set; }
        public override int ActionSeatPosition { get; set; }
        public override int DealerButton { get; set; }
        public override int StartDealingAtSeatNumber { get ; set ; }
        public override List<Card> Community { get; set; } = new List<Card>();
        public override int TableId { get; set ; }
        public override int Seats { get; set; }
        public override double Pot { get; set; }
        public override StreetName Street { get; set; } = StreetName.PreFlop;
        public override int PlayerTimeout { get; set; }
        public override Task RunningGame { get; set; }
        public override Deck Deck { get; set; }
        public override double LastBet { get; set; }
        public override double MinBet { get; set; }
        public override CancellationTokenSource GameCancellationSource { get; set; }

        public TexasHoldemNoLimit(
            int tableId,
            Deck deck,
            double smallBlind,
            double bigBlind,
            int seats = 9,
            int dealerButton = 1)
        {
            TableId = tableId;
            Deck = deck;
            _smallBlind = smallBlind;
            _bigBlind = bigBlind;
            DealerButton = dealerButton;
            StartDealingAtSeatNumber = dealerButton;
            ActionSeatPosition = dealerButton;
            Seats = seats;
            Street = StreetName.PreFlop;
        }

        public override void StartPlayerAction()
        {
            ActionSeatPosition = DealerButton;

            base.StartPlayerAction();
        }
    }
}
