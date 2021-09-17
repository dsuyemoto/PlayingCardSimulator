using System.Collections.Generic;

namespace Dealer
{
    public interface IDeck
    {
        List<Card> Cards { get; set; }
        Hand BestHand(List<Hand> hands);
        Card GetCard(int slot);
        Card GetRandomCard();
    }
}