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
        public double Chips { get; set; }
        public double Bet { get; set; }
        public PlayerAction LastAction { get; set; } = PlayerAction.None;
        public PlayerAction CurrentAction { get; set; } = PlayerAction.None;
        public PromptOptions Options { get; set; } = new PromptOptions();
        public int SeatNumber { get; set; }
        public bool SitOut { get; set; } = false;

        public Player(int id, double chips)
        {
            Id = id;
            Chips = chips;
        }
    }
}
