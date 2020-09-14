using System.Collections.Generic;

namespace Dealer
{
    public class TexasHoldemCash : TexasHoldemBase
    {
        TexasHoldemBase _texasHoldemBase;

        public override int DealerButtonSeatNumber
        {
            get { return _texasHoldemBase.DealerButtonSeatNumber; }
            set { _texasHoldemBase.DealerButtonSeatNumber = value; }
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

        public override decimal Pot 
        {
            get { return _texasHoldemBase.Pot; }
            set { _texasHoldemBase.Pot = value; }
        }

        public override double PlayerTimeoutMilliseconds
        {
            get { return _texasHoldemBase.PlayerTimeoutMilliseconds; }
            set { _texasHoldemBase.PlayerTimeoutMilliseconds = value; }
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
        public override bool AutoStartEnabled 
        {
            get { return _texasHoldemBase.AutoStartEnabled; }
            set { _texasHoldemBase.AutoStartEnabled = value; }
        }

        public TexasHoldemCash(TexasHoldemBase texasHoldemBase)
        {
            _texasHoldemBase = texasHoldemBase;
            _texasHoldemBase.AutoStartEnabled = true;
        }

        protected override TableViewBase GetTableView(int playerId)
        {
            return new TexasHoldemView(this, playerId);
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

        protected override void SetBlindBet(decimal blind, int seatNumber)
        {
            var player = GetPlayer(seatNumber);
            if (player.Chips < blind)
            {
                SitOut(seatNumber);
                return;
            }

            base.SetBlindBet(blind, seatNumber);
        }
        protected void AutoStartGame()
        {
            if (!AutoStartEnabled) return;

            if (!IsGameRunning && GetSittingPlayers().Count >= 2)
                StartGame();
            else if (IsGameRunning && GetSittingPlayers().Count < 2)
                StopGame();
        }

    }
}
