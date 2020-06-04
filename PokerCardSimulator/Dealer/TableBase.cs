using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dealer
{
    public abstract class TableBase
    {
        protected Deck _deck;

        protected int TotalStreets { get; set; }
        protected int HoleCards { get; set; }
        protected int DealerStart { get; set; }
        public int Street { get; set; } = 0;
        public List<Seat> Seats { get; } = new List<Seat>();
        
        protected void DealHoleCards()
        {
            var rounds = 0;
            while (rounds < HoleCards)
            {
                var cardPosition = DealerStart;
                var peopleDealt = 0;
                while (peopleDealt < Seats.Count)
                {
                    if (Seats.Exists(s => s.Number == cardPosition))
                    {
                        var seat = Seats.First(s => s.Number == cardPosition);
                        var card = _deck.GetRandomCard();
                        card.IsHidden = true;
                        seat.Cards.Add(card);
                        peopleDealt++;
                    }
                    cardPosition++;
                }
                rounds++;
            }
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (Seats.Count > 0 && Seats.Exists(s => s.Number == seatNumber)) return false;

            Seats.Add(new Seat(seatNumber) { Player = player });

            return true;
        }
    }
}
