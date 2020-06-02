using System;
using System.Collections.Generic;
using System.Text;

namespace CardDeck
{
    public class TexasHoldem
    {
        Deck _deck;

        const int STREETS = 3;
        const int HOLECARDS = 2;
        public int Street { get; set; } = 0;
        public Seat[] Seats { get; set; }
        public int DealerButton { get; set; }

        public TexasHoldem(Deck deck, int seats)
        {
            _deck = deck;
            Seats = new Seat[seats];
        }

        public void Deal()
        {
            if (Street < 1)
            {
                var rounds = 0;
                while (rounds < HOLECARDS)
                {
                    var cardsDealt = 0;
                    var cardPosition = DealerButton + 1;

                    while (cardsDealt < Seats.Length)
                    {
                        if (cardPosition > Seats.Length - 1)
                            cardPosition = 0;
                        if (Seats[cardPosition].Player != null)
                            Seats[cardPosition].Cards.Add(_deck.GetRandomCard());
                        cardsDealt++;
                        cardPosition++;
                    }
                }
            }
        }
    }
}
