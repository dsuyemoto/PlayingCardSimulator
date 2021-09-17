using System;
using System.Collections.Generic;

namespace Dealer
{
    public class Player
    {
        EventHandler _actionPrompted;

        private void ActionPromptedhandler(object sender, EventArgs eventArgs)
        {
            _actionPrompted(sender, eventArgs);
        }

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
        public bool SittingOut { get; set; } = false;
        public int Countdown { get; set; }
        public TableViewBase ReturnView { get; set; }
        public System.Timers.Timer Timer { get; set; }

        event EventHandler ActionPrompted;
        event EventHandler PlayerActed;

        public Player(int id, EventHandler actionPrompted)
        {
            Id = id;
            _actionPrompted = actionPrompted;
        }

        public void OnActionPrompted(object sender, ActionPromptedEventArgs actionPromptedEventArgs)
        {
            var handler = ActionPrompted;
            handler?.Invoke(sender, actionPromptedEventArgs);
        }

        public void OnPlayerActed(object sender, EventArgs e)
        {
            var handler = PlayerActed;
            handler?.Invoke(sender, e);
        }

        public void ResetActionPrompted()
        {
            ActionPrompted += _actionPrompted;
        }
    }
}
