using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Dealer
{
    public class TexasHoldemCash : TexasHoldemBase
    {
        TexasHoldemBase _texasHoldemBase;

        public override int ActionSeatPosition 
        {
            get { return _texasHoldemBase.ActionSeatPosition; }
            set { _texasHoldemBase.ActionSeatPosition = value; }
        }

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

        public override int TableId {
            get { return _texasHoldemBase.TableId; }
            set { _texasHoldemBase.TableId = value; }
        }

        public override int Seats 
        {
            get { return _texasHoldemBase.Seats; }
            set { _texasHoldemBase.Seats = value; }
        }

        public override double Pot 
        {
            get { return _texasHoldemBase.Pot; }
            set { _texasHoldemBase.Pot = value; }
        }

        public override StreetName Street 
        {
            get { return _texasHoldemBase.Street; }
            set { _texasHoldemBase.Street = value; }
        }

        public override List<StreetBase> Streets
        {
            get { return _texasHoldemBase.Streets; }
            set { _texasHoldemBase.Streets = value; }
        }

        public override int StreetCount
        {
            get { return _texasHoldemBase.StreetCount; }
            set { _texasHoldemBase.StreetCount = value; }
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

        public override CancellationTokenSource GameCancellationSource 
        {
            get { return _texasHoldemBase.GameCancellationSource; }
            set { _texasHoldemBase.GameCancellationSource = value; }
        }

        public override Deck Deck
        {
            get { return _texasHoldemBase.Deck; }
            set { _texasHoldemBase.Deck = value; }
        }

        public override double LastBet
        {
            get { return _texasHoldemBase.LastBet; }
            set { _texasHoldemBase.LastBet = value; }
        }

        public override double MinBet
        {
            get { return _texasHoldemBase.MinBet; }
            set { _texasHoldemBase.MinBet = value; }
        }

        public override List<Player> Players
        {
            get { return _texasHoldemBase.Players; }
            set { _texasHoldemBase.Players = value; }
        }

        public TexasHoldemCash(TexasHoldemBase texasHoldemBase)
        {
            _texasHoldemBase = texasHoldemBase;
        }

        public override TexasHoldemView GetTableView(int playerId)
        {
            return _texasHoldemBase.GetTableView(playerId);
        }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            var succeeds = base.SeatPlayer(player, seatNumber);
            AutoStartGame();

            return succeeds;
        }

        public override bool UnseatPlayer(int seatNumber)
        {
            var succeeds = base.UnseatPlayer(seatNumber);
            AutoStartGame();

            return succeeds;
        }

        public override void SitIn(int seatNumber)
        {
            base.SitIn(seatNumber);
            AutoStartGame();
        }

        public override void SitOut(int seatNumber)
        {
            base.SitOut(seatNumber);
            AutoStartGame();
        }

        protected override void SetBlindBets(double blind, int seatNumber)
        {
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            if (Players[playerIndex].Chips > blind)
            {
                Players[playerIndex].SitOut = true;
                return;
            }

            base.SetBlindBets(blind, seatNumber);
        }
        protected void AutoStartGame()
        {
            if (!IsGameRunning && Players.FindAll(p => p.SitOut = false).Count >= 2)
                StartGame();
            else if (IsGameRunning && Players.FindAll(p => p.SitOut = false).Count < 2)
                StopGame();
        }

    }
}
