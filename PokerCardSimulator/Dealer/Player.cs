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

        public int Id { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public double Chips { get; set; }
        public double Bet { get; set; }
        public PlayerAction Action { get; set; }
        public PlayerAction[] Options { get; set; }
        public double MinBet { get; set; }

        public Player(int id, double chips)
        {
            Id = id;
            Chips = chips;
        }
    }
}
