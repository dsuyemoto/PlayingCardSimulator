using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dealer
{
    public class TexasHoldemTournament : TexasHoldemBase
    {
        TexasHoldemBase _texasHoldemBase;

        public override int DealerButtonSeatNumber
        {
            get { return _texasHoldemBase.DealerButtonSeatNumber; }
            set { _texasHoldemBase.DealerButtonSeatNumber = value; }
        }
        public override int BigBlindSeatNumber
        {
            get { return _texasHoldemBase.BigBlindSeatNumber; }
            set { _texasHoldemBase.BigBlindSeatNumber = value; }
        }
        public override int SmallBlindSeatNumber
        {
            get { return _texasHoldemBase.SmallBlindSeatNumber; }
            set { _texasHoldemBase.SmallBlindSeatNumber = value; }
        }
        public override int StartDealingSeatNumber
        {
            get { return _texasHoldemBase.StartDealingSeatNumber; }
            set { _texasHoldemBase.StartDealingSeatNumber = value; }
        }
        public override List<Card> Community
        {
            get { return _texasHoldemBase.Community; }
            set { _texasHoldemBase.Community = value; }
        }
        public override int TableId
        {
            get { return _texasHoldemBase.TableId; }
            set { _texasHoldemBase.TableId = value; }
        }
        public override int Seats
        {
            get { return _texasHoldemBase.Seats; }
            set { _texasHoldemBase.Seats = value; }
        }
        public override decimal Pot
        {
            get { return _texasHoldemBase.Pot; }
            set { _texasHoldemBase.Pot = value; }
        }
        public override Streets Streets
        {
            get { return _texasHoldemBase.Streets; }
            set { _texasHoldemBase.Streets = value; }
        }
        public override int PlayerTimeout
        {
            get { return _texasHoldemBase.PlayerTimeout; }
            set { _texasHoldemBase.PlayerTimeout = value; }
        }
        public override Task RunningGame
        {
            get { return _texasHoldemBase.RunningGame; }
            set { _texasHoldemBase.RunningGame = value; }
        }
        public override Deck Deck
        {
            get { return _texasHoldemBase.Deck; }
            set { _texasHoldemBase.Deck = value; }
        }
        public override decimal LastBet
        {
            get { return _texasHoldemBase.LastBet; }
            set { _texasHoldemBase.LastBet = value; }
        }
        public override CancellationTokenSource GameCancellationSource
        {
            get { return _texasHoldemBase.GameCancellationSource; }
            set { _texasHoldemBase.GameCancellationSource = value; }
        }
        public override decimal SmallBlind
        {
            get { return _texasHoldemBase.SmallBlind; }
            set { _texasHoldemBase.SmallBlind = value; }
        }
        public override decimal BigBlind
        {
            get { return _texasHoldemBase.BigBlind; }
            set { _texasHoldemBase.BigBlind = value; }
        }
        public override List<Player> Players => _texasHoldemBase.Players;

        public TexasHoldemTournament(TexasHoldemBase tableBase)
        {
            _texasHoldemBase = tableBase;
        }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            return _texasHoldemBase.SeatPlayer(player, seatNumber);
        }

        public override void DealHand()
        {
            _texasHoldemBase.DealHand();
        }

        protected override void SetBlindBet(decimal blind, int seatNumber)
        {
            var player = GetPlayer(seatNumber);
            if (player.Chips > blind)
            {
                player.Bet = player.Chips;
                player.Chips = 0;
                UpdatePlayer(player);
                return;
            }

            base.SetBlindBet(blind, seatNumber);
        }

        protected override TableViewBase GetTableView(int playerId)
        {
            return new TexasHoldemView(this, playerId);
        }
    }
}
