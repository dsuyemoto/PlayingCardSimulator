using System;
using System.Collections.Generic;
using static Dealer.TableBase;

namespace Dealer
{
    public class Player
    {
        public enum PlayerAction
        {
            Check,
            Bet,
            Call,
            Fold
        }

        public int Id { get; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public double Chips { get; set; }
        public double Bet { get; set; }
        public PlayerAction Action { get; set; }
        public PromptOptions Options { get; set; } = new PromptOptions();
        public Func<PromptOptions, PromptActions> Prompt { get; set; }
        public int SeatNumber { get; set; }
        public bool SitOut { get; set; } = false;

        public Player(int id, double chips, Func<PromptOptions, PromptActions> prompt)
        {
            Id = id;
            Chips = chips;
            Prompt = prompt;
        }
    }
}
