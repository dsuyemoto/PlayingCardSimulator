using System;
using System.Collections.Generic;
using System.Linq;
using static Dealer.Player;

namespace Dealer
{
    public abstract class TableBase
    {
        protected Deck _deck;
        protected int _actionSeatPosition;
        protected double _lastBet;
        protected double _minBet;

        public enum StreetName
        {
            PreFlop,
            Flop,
            Turn,
            River
        }

        public int DealerButton { get; protected set; }
        public List<Player> Players { get; protected set; } = new List<Player>();
        public int Seats { get; protected set; }
        public List<Card> Community { get; protected set; } = new List<Card>();
        public double Pot { get; protected set; }
        public double SmallBlind { get; protected set; }
        public double BigBlind { get; protected set; }
        public StreetName Street { get; protected set; } = StreetName.PreFlop;
        public bool IsTournament { get; set; }

        protected void DealHoleCards(int number)
        {
            var dealtCards = 0;
            while (dealtCards < number)
            {
                DealPlayerCards(true);
                dealtCards++;
            }
        }

        protected void DealPlayerCards(bool isHidden)
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

        protected void DealCommunityCards(int number)
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

        public virtual bool SeatPlayer(Player player, int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (Players.Exists((p)=> p.SeatNumber == seatNumber)) return false;
            
            player.SeatNumber = seatNumber;
            Players.Add(player);
            
            return true;
        }

        public virtual bool UnseatPlayer(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (!Players.Exists((p)=> p.SeatNumber == seatNumber)) return false;

            Players.Remove(Players.Single((p)=> p.SeatNumber == seatNumber));
                        
            return true;
        }

        public void GetPlayerAction()
        {
            int playersActed = 0;
            _actionSeatPosition = DealerButton;
            if (Players.Count > 2)
            {
                IncrementActionSeat();
                if (SmallBlind > 0)
                    IncrementActionSeat();
                if (BigBlind > 0)
                    IncrementActionSeat();
            }
            while (playersActed < Players.Count)
            {
                var player = GetPlayerToAct();
                var result = player.Prompt(player.Options);
                var playerIndex = Players.FindIndex(p => p.SeatNumber == player.SeatNumber);
                Players[playerIndex].Action = result.PlayerAction;

                if (result.PlayerAction == PlayerAction.Call)
                {
                    Players[playerIndex].Bet = _lastBet;
                }
                else if (result.PlayerAction == PlayerAction.Bet)
                {
                    Players[playerIndex].Bet = result.Bet;
                    _minBet = (result.Bet * 2) - _lastBet;
                    _lastBet = result.Bet;
                }

                playersActed++;
            }
        }

        protected abstract Player GetPlayerToAct();

        protected void IncrementActionSeat()
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false) as List<Player>;
            do
            {
                _actionSeatPosition++;
                if (_actionSeatPosition > Seats)
                    _actionSeatPosition = 1;
            }
            while (!activePlayers.Exists(p => p.SeatNumber == _actionSeatPosition));
        }

        protected bool CollectBets()
        {
            var activePlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var activePlayer in activePlayers)
                Pot += activePlayer.Bet;
            if (activePlayers.Count > 1)
                return true;
            return false;
        }

        protected void SetBlinds()
        {
            if (Players.Count < 2) return;

            var seatPosition = DealerButton + 1;
            if (Players.Count == 2)
                seatPosition = DealerButton;
            var smallBlindAssigned = false;
            var bigBlindAssigned = false;
            while (!smallBlindAssigned || !bigBlindAssigned)
            {
                if (seatPosition > Seats)
                    seatPosition = 1;
                if (Players.Exists(p => p.SeatNumber == seatPosition))
                {
                    var playerIndex = Players.FindIndex((p) => p.SeatNumber == seatPosition);
                    if (!smallBlindAssigned)
                    {
                        SetBlind(SmallBlind, playerIndex);
                        smallBlindAssigned = true;
                    }
                    else if (!bigBlindAssigned)
                    {
                        SetBlind(BigBlind, playerIndex);
                        bigBlindAssigned = true;
                        _lastBet = BigBlind;
                        _minBet = BigBlind * 2;
                    }
                }

                seatPosition++;
            }
        }

        private void SetBlind(double blind, int playerIndex)
        {
            if (Players[playerIndex].Chips - blind < 0)
            {
                if (IsTournament)
                {
                    Players[playerIndex].Bet = Players[playerIndex].Chips;
                    Players[playerIndex].Chips = 0;
                }
                else
                {
                    Players[playerIndex].SitOut = true;
                    return;
                }
            }
            else
            {
                Players[playerIndex].Chips -= blind;
                Players[playerIndex].Bet = blind;
            }
        }
    }
}
