using System;
using System.Collections.Generic;

namespace Dealer
{
    public class Player
    {
        public enum PlayerAction
        {
            Check,
            Bet,
            Call,
            Fold,
            None
        }

        public readonly int Id;
        public List<Card> Cards { get; set; } = new List<Card>();
        public decimal Chips { get; set; }
        public decimal Bet { get; set; }
        public PlayerAction CurrentAction { get; set; } = PlayerAction.None;
        public PlayerOptions Options { get; set; } = new PlayerOptions();
        public int SeatNumber { get; set; }
        public bool SitOut { get; set; } = false;
        public int Countdown { get; set; }
        public TableViewBase ReturnView { get; set; }

        public event EventHandler WaitForTurn;
        public event EventHandler WaitForPlayerResponse;

        public Player(int id)
        {
            Id = id;
        }

        protected virtual void OnWaitForTurn(object sender)
        {
            var handler = WaitForTurn;
            handler?.Invoke(sender, EventArgs.Empty);
        }

        protected virtual void OnWaitForPlayerResponse(object sender, EventArgs e)
        {
            var handler = WaitForPlayerResponse;
            handler?.Invoke(sender, e);
        }

        public void Notify(TableBase tableBase)
        {
            OnWaitForTurn(tableBase);
        }
    }
}
