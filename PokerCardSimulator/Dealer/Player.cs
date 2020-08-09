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

        public enum BlindName
        {
            Small,
            Big
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
        public System.Timers.Timer Timer { get; set; }

        public event EventHandler ActionPrompted;
        public event EventHandler PlayerActed;

        public Player(int id)
        {
            Id = id;
        }

        public virtual void OnActionPrompted(object sender, ActionPromptedEventArgs actionPromptedEventArgs)
        {
            var handler = ActionPrompted;
            handler?.Invoke(sender, actionPromptedEventArgs);
        }

        public virtual void OnPlayerActed(object sender, EventArgs e)
        {
            var handler = PlayerActed;
            handler?.Invoke(sender, e);
        }
    }
}
