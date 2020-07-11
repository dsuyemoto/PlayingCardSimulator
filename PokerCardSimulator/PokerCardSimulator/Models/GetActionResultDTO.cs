using Dealer;
using System.Collections.Generic;

namespace PokerCardSimulator.Models
{
    public class GetActionResultDTO
    {
        public string[] AllowedActions { get; set; }
        public double MinBet { get; set; }

        public GetActionResultDTO(PromptOptions promptOptions)
        {
            var allowedActions = new List<string>();
            foreach (var allowedAction in promptOptions.AllowedActions)
                allowedActions.Add(allowedAction.ToString());
            AllowedActions = allowedActions.ToArray();
            MinBet = promptOptions.MinBet;
        }
    }
}
