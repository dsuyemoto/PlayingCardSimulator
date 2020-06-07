using System;
using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public abstract class TableBase
    {
        protected Deck _deck;

        protected int TotalStreets { get; set; }
        protected int HoleCards { get; set; }
        protected int DealerButton { get; set; }
        public int StreetCount { get; set; } = 0;
        public Player[] SeatedPlayers { get; protected set; }
        
        protected void DealHoleCards()
        {
            var dealtCards = 0;
            while (dealtCards < HoleCards)
            {
                var seatPosition = DealerButton + 1;
                var peopleDealt = 0;
                while (peopleDealt < SeatedPlayers.Length)
                {
                    if (SeatedPlayers[seatPosition] != null)
                    {
                        var card = _deck.GetRandomCard();
                        card.IsHidden = true;
                        SeatedPlayers[seatPosition].Cards.Add(card);
                        peopleDealt++;
                    }
                    seatPosition++;
                }
                dealtCards++;
            }
        }

        public bool Bet()
        {
            return false;
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (SeatedPlayers[seatNumber] != null) return false;

            SeatedPlayers[seatNumber] = player;

            return true;
        }

        public bool UnseatPlayer(int seatNumber)
        {
            if (SeatedPlayers[seatNumber] == null) return false;

            SeatedPlayers[seatNumber] = null;

            return true;
        }

        public List<int> GetAvailableSeats()
        {
            var emptySeats = new List<int>();
            for (var i = 0; i < SeatedPlayers.Length; i++)
                if (SeatedPlayers[i] == null)
                    emptySeats.Add(i);

            return emptySeats;
        }
    }
}
