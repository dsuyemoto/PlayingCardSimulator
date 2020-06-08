using System;
using static Dealer.Player;

namespace Dealer
{
    public class TexasHoldem : TableBase
    {
        public enum StreetName
        {
            PreFlop = 0,
            Flop = 1,
            Turn = 2,
            River = 3
        }

        public double SmallBlind { get; set; }
        public double BigBlind { get; set; }
        public StreetName Street { get; set; } = StreetName.PreFlop;

        public TexasHoldem(
            Deck deck, 
            double smallBlind,
            double bigBlind, 
            int seats = 9,
            int dealerButton = 0)
        {
            _deck = deck;
            HoleCards = 2;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Seats = new Player[seats];
            DealerButton = dealerButton;
            Community = new Card[5];
        }

        public bool StartHand()
        {
            SetBlinds();
            return Deal();
        }

        public bool Deal()
        {
            if (Array.FindAll(Seats, s => s != null).Length < 2) return false;

            if (Street == StreetName.PreFlop)
            {
                DealHoleCards();
                Street = StreetName.Flop;

                return true;
            }
            else if (Street == StreetName.Flop)
            {
                var cardsDealt = 0;
                while (cardsDealt < 3)
                {
                    DealCommunityCards(cardsDealt);
                    cardsDealt++;
                }
                Street = StreetName.Turn;

                return true;
            }
            else if (Street == StreetName.Turn || Street == StreetName.River)
            {
                DealCommunityCards(3);
                Street = StreetName.River;

                return true;
            }

            return false;
        }

        public Player GetPlayer(Player previousPlayer)
        {
            Player player = null;
            while (player == null || player.Action == PlayerAction.Fold)
            {
                player = Seats[CardPosition];
                CardPosition++;
                if (CardPosition == Seats.Length)
                    CardPosition = 0;
                if (player != null)
                {
                    if (previousPlayer.Bet > 0)
                        player.Options = new PlayerAction[] {
                        PlayerAction.Bet,
                        PlayerAction.Call,
                        PlayerAction.Fold
                    };
                    else
                        player.Options = new PlayerAction[]
                        {
                        PlayerAction.Bet,
                        PlayerAction.Check,
                        PlayerAction.Fold
                        };
                }
            }
            if (player != null)
            {
                player.MinBet = BigBlind;
                if (previousPlayer.Bet > 0)
                {
                    if (previousPlayer.Bet > BigBlind)
                        player.MinBet = (previousPlayer.Bet - previousPlayer.MinBet) + previousPlayer.Bet;
                }
            }
            return player;
        }

        public bool ContinueBetting()
        {
            var activePlayers = Array.FindAll(Seats, s => s.Action != PlayerAction.Fold);
            foreach (var activePlayer in activePlayers)
                Pot = Pot + activePlayer.Bet;
            if (Array.FindAll(Seats, s => s.Action != PlayerAction.Fold).Length > 1)
                return true;
            return false;
        }

        private void SetBlinds()
        {
            var position = DealerButton + 1;
            var seatCount = 1;
            while (seatCount < 3)
            {
                if (position == Seats.Length)
                    position = 0;
                if (Seats[position] != null)
                    if (seatCount == 1)
                    {
                        Seats[position].Bet = SmallBlind;
                        seatCount++;
                    }
                    else if (seatCount == 2)
                    {
                        Seats[position].Bet = BigBlind;
                        seatCount++;
                    }
                position++;
            }
        }

        private void DealCommunityCards(int position)
        {
            var card = _deck.GetRandomCard();
            card.IsHidden = false;
            Community[position] = card;
        }
    }
}
