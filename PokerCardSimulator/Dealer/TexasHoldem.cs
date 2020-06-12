using System;
using System.Linq;
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
            DealerButton = dealerButton;
            Seats = seats;
        }

        public void StartHand()
        {
            SetBlinds();
            Deal();
            Action();
        }

        private void SetBlinds()
        {
            var position = DealerButton + 1;
            var seatCount = 1;
            while (seatCount < 3)
            {
                if (position > Seats)
                    position = 1;
                if (Players.Exists((p)=> p.SeatNumber == position))
                    if (seatCount == 1)
                    {
                        Players.Single((p)=> p.SeatNumber == position).Bet = SmallBlind;
                        seatCount++;
                    }
                    else if (seatCount == 2)
                    {
                        Players.Single((p)=> p.SeatNumber == position).Bet = BigBlind;
                        seatCount++;
                    }
                position++;
            }
        }

        public bool Deal()
        {
            if (Players.Count < 2) return false;

            if (Street == StreetName.PreFlop)
            {
                DealHoleCards();
                Street = StreetName.Flop;

                return true;
            }
            else if (Street == StreetName.Flop)
            {
                DealCommunityCards(3);
                Street = StreetName.Turn;

                return true;
            }
            else if (Street == StreetName.Turn || Street == StreetName.River)
            {
                DealCommunityCards(1);
                Street = StreetName.River;

                return true;
            }

            return false;
        }

        private void Action()
        {
            Player previousPlayer = null;
            int playersActed = 0;
            while (playersActed < Players.Count)
            {
                var player = GetPlayer(previousPlayer);
                var result = player.Prompt(player.Options);
                player.Action = result.PlayerAction;
                player.Bet = result.Bet;
                previousPlayer = player;
                playersActed++;
            }
        }

        public Player GetPlayer(Player previousPlayer)
        {
            Player player = null;
            var actionSeatPosition = DealerButton + 1;
            if (Street == StreetName.PreFlop)
                actionSeatPosition = DealerButton + 3;
            int seatCount = 1;
            while (seatCount < Seats)
            {
                if (actionSeatPosition == Seats)
                    actionSeatPosition = 1;

                if (Players.Exists((p) => p.SeatNumber == actionSeatPosition))
                {
                    player = Players.Single((p) => p.SeatNumber == actionSeatPosition);
                    player.Options.PreviousBet = 0;
                    player.Options.MinBet = BigBlind;
                    player.Options.AllowedActions = new PlayerAction[]
                    {
                        PlayerAction.Bet,
                        PlayerAction.Check,
                        PlayerAction.Fold
                    };

                    if (Street == StreetName.PreFlop)
                    {
                        player.Options.PreviousBet = BigBlind;
                        player.Options.MinBet = BigBlind * 2;
                        player.Options.AllowedActions = new PlayerAction[]
                        {
                            PlayerAction.Bet,
                            PlayerAction.Call,
                            PlayerAction.Fold
                        };
                    }

                    if (previousPlayer != null)
                    {
                        player.Options.PreviousBet = previousPlayer.Bet;
                        if (previousPlayer.Bet > BigBlind)
                            player.Options.MinBet =
                                previousPlayer.Bet * 2 - 
                                previousPlayer.Options.PreviousBet;

                        if (previousPlayer.Bet < previousPlayer.Options.MinBet)
                        {
                            player.Options.AllowedActions = new PlayerAction[]
                            {
                                PlayerAction.Bet,
                                PlayerAction.Call,
                                PlayerAction.Fold
                            };
                            player.Options.MinBet = previousPlayer.Options.MinBet;
                        }
                    }
                }

                actionSeatPosition++;
                seatCount++;
            }

            return player;
        }

        private bool CollecBets()
        {
            var activePlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var activePlayer in activePlayers)
                Pot = Pot + activePlayer.Bet;
            if (activePlayers.Count > 1)
                return true;
            return false;
        }

        private void DealCommunityCards(int number)
        {
            var cardCount = 0;
            while (cardCount < number)
            {
                var card = _deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                cardCount++;
            }
        }
    }
}
