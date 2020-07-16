using System;
using System.Collections.Generic;

namespace Dealer
{
    public class Player : IObserver<PlayerEvent>
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
        public double Chips { get; set; }
        public double Bet { get; set; }
        public PlayerAction CurrentAction { get; set; } = PlayerAction.None;
        public PromptOptions Options { get; set; } = new PromptOptions();
        public int SeatNumber { get; set; }
        public bool SitOut { get; set; } = false;
        public int Countdown { get; set; }

        public Player(int id, double chips)
        {
            Id = id;
            Chips = chips;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PlayerEvent value)
        {
            Options = value.Options;
        }
    }
}
