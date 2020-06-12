using static Dealer.Player;

namespace Dealer
{
    public class PromptOptions
    {
        public PlayerAction[] AllowedActions { get; set; }
        public double MinBet { get; set; }
        public double PreviousBet { get; set; }
    }
}