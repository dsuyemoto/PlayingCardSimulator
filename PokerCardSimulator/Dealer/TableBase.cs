using System;
using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public abstract class TableBase
    {
        protected Deck _deck;
      
        protected int HoleCards { get; set; }
        protected int DealerButton { get; set; }
        public Player[] Seats { get; protected set; }
        public Card[] Community { get; set; }
        protected int CardPosition { get; set; }
        public double Pot { get; set; }

        protected void DealHoleCards()
        {
            var dealtCards = 0;
            while (dealtCards < HoleCards)
            {
                var seatPosition = DealerButton + 1;
                var peopleDealt = 0;
                while (peopleDealt < Seats.Length)
                {
                    if (Seats[seatPosition] != null)
                    {
                        var card = _deck.GetRandomCard();
                        card.IsHidden = true;
                        Seats[seatPosition].Cards.Add(card);
                        peopleDealt++;
                    }
                    seatPosition++;
                }
                dealtCards++;
            }
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (Seats[seatNumber] != null) return false;

            Seats[seatNumber] = player;

            return true;
        }

        public bool UnseatPlayer(int seatNumber)
        {
            if (Seats[seatNumber] == null) return false;

            Seats[seatNumber] = null;

            return true;
        }

        public List<int> GetAvailableSeats()
        {
            var emptySeats = new List<int>();
            for (var i = 0; i < Seats.Length; i++)
                if (Seats[i] == null)
                    emptySeats.Add(i);

            return emptySeats;
        }
    }
}
