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
        public List<Player> Players { get; protected set; } = new List<Player>();
        public int Seats { get; protected set; }
        public List<Card> Community { get; set; } = new List<Card>();
        public double Pot { get; set; }

        protected void DealHoleCards()
        {
            var dealtCards = 0;
            while (dealtCards < HoleCards)
            {
                DealCards(true);
                dealtCards++;
            }
        }

        protected void DealCards(bool isHidden)
        {
            var seatNumber = DealerButton + 1;
            int seatCount = 0;
            while (seatCount < Seats)
            {
                var playerIndex = Players.FindIndex(0, p => p.SeatNumber == seatNumber);
                if (playerIndex >= 0)
                {
                    var card = _deck.GetRandomCard();
                    card.IsHidden = isHidden;
                    Players[playerIndex].Cards.Add(card);
                }

                seatCount++;
                seatNumber++;
                if (seatNumber > Seats)
                    seatNumber = 1;
            }
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (Players.Exists((p)=> p.SeatNumber == seatNumber)) return false;

            player.SeatNumber = seatNumber;
            Players.Add(player);

            return true;
        }

        public bool UnseatPlayer(int seatNumber)
        {
            if (!Players.Exists((p)=> p.SeatNumber == seatNumber)) return false;

            Players.Remove(Players.Single((p)=> p.SeatNumber == seatNumber));

            return true;
        }
    }
}
