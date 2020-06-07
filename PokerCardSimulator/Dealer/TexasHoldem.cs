using System;
using System.Collections.Generic;

namespace Dealer
{
    public class TexasHoldem : TableBase
    {
        private enum Street
        {
            HoleCards = 0,
            Flop = 1,
            Turn = 2,
            River = 3
        }

        public List<Card> Community { get; set; } = new List<Card>();
        public double SmallBlind { get; set; }
        public double BigBlind { get; set; }

        public TexasHoldem(
            Deck deck, 
            double smallBlind,
            double bigBlind, 
            int seats = 9,
            int dealerButton = 0)
        {
            _deck = deck;
            TotalStreets = 3;
            HoleCards = 2;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            SeatedPlayers = new Player[seats];
            DealerButton = dealerButton;
        }

        public bool Deal()
        {
            if (Array.FindAll(SeatedPlayers, s => s != null).Length < 2) return false;

            if (StreetCount == (int)Street.HoleCards)
            {
                DealHoleCards();
                StreetCount++;

                return true;
            }
            else if (StreetCount == (int)Street.Flop)
            {
                var cardsDealt = 1;
                while (cardsDealt < 4)
                {
                    var card = _deck.GetRandomCard();
                    card.IsHidden = false;
                    Community.Add(card);
                    cardsDealt++;
                }
                StreetCount++;

                return true;
            }
            else if (StreetCount == (int)Street.Turn)
            {
                var card = _deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                StreetCount++;

                return true;
            }
            else if (StreetCount == (int)Street.River)
            {
                var card = _deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                StreetCount++;

                return true;
            }

            return false;
        }

        private double GetBlinds(int seatPosition)
        {
            var smallBlindPosition = DealerButton + 1;
            if (smallBlindPosition > SeatedPlayers.Length)
                smallBlindPosition = smallBlindPosition - SeatedPlayers.Length;
            var bigBlindPosition = DealerButton + 2;
            if (bigBlindPosition > SeatedPlayers.Length)
                bigBlindPosition = bigBlindPosition - SeatedPlayers.Length;

            if (seatPosition == smallBlindPosition)
                return SmallBlind;
            else if (seatPosition == bigBlindPosition)
                return BigBlind;

            return 0;
        }
    }
}
