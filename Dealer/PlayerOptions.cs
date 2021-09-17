using System.Collections.Generic;
using static Dealer.Player;

namespace Dealer
{
    public class PlayerOptions
    {
        public PlayerAction[] AllowedActions { get; set; }
        public decimal MinBet { get; set; }
    }
}