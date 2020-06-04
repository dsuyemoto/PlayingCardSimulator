using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public class TexasHoldem : TableBase
    {
        public List<Card> Community { get; set; } = new List<Card>();

        public TexasHoldem(Deck deck, int dealstart = 0)
        {
            _deck = deck;
            TotalStreets = 3;
            HoleCards = 2;
            DealerStart = dealstart;
        }

        public bool Deal()
        {
            if (Seats.FindAll(s => s.Player != null).Count < 2) return false;

            if (Street == 0)
            {
                DealHoleCards();
                Street++;

                return true;
            }
            else if (Street == 1)
            {
                var cardsDealt = 1;
                while (cardsDealt < 4)
                {
                    var card = _deck.GetRandomCard();
                    card.IsHidden = false;
                    Community.Add(card);
                    cardsDealt++;
                }
                Street++;

                return true;
            }
            else if (Street == 2)
            {
                var card = _deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                Street++;

                return true;
            }
            else if (Street == 3)
            {
                var card = _deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                Street++;

                return true;
            }

            return false;
        }
    }
}
