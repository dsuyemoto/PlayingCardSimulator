using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Dealer.Player;

namespace Dealer
{
    public abstract class TableBase
    {
        public enum StreetName
        {
            PreFlop,
            Flop,
            Turn,
            River
        }

        public abstract int TableId { get; set; }
        public abstract int Seats { get; set; }
        public abstract double Pot { get; set; }
        public abstract StreetName Street { get; set; }
        public abstract List<StreetBase> Streets { get;set;}
        public abstract int StreetCount { get; set; }
        public abstract int PlayerTimeout { get; set; }
        public bool IsGameRunning => GetGameStatus();
        public abstract List<Player> Players { get; set; }
        public abstract int ActionSeatPosition { get; set; }
        public abstract int StartDealingSeatNumber { get; set; }
        public abstract Task RunningGame { get; set; }
        public abstract Deck Deck { get; set; }
        public abstract double LastBet { get; set; }
        public abstract double MinBet { get; set; }
        public abstract CancellationTokenSource GameCancellationSource { get; set; }

        public virtual bool SeatPlayer(Player player, int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (Players.Exists((p) => p.SeatNumber == seatNumber)) return false;

            player.SeatNumber = seatNumber;
            player.SitOut = true;
            Players.Add(player);

            if (Players.Count == 1)
                StartDealingSeatNumber = seatNumber;

            return true;
        }

        public virtual bool UnseatPlayer(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (!Players.Exists(p => p.SeatNumber == seatNumber)) return false;

            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            Players.RemoveAt(playerIndex);

            return true;
        }

        public virtual void SitOut(int seatNumber)
        {
            if (Players.Exists(p => p.SeatNumber == seatNumber))
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
                Players[playerIndex].SitOut = true;
            }
        }

        public virtual void SitIn(int seatNumber)
        {
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            Players[playerIndex].SitOut = false;
        }

        public void StartGame()
        {
            GameCancellationSource = new CancellationTokenSource();

            RunningGame = Task.Run(() =>
            {
                while (!GameCancellationSource.Token.IsCancellationRequested && Players.FindAll(p => p.SitOut = false).Count >= 2)
                {
                    DealHand();
                }
            }, GameCancellationSource.Token);
        }

        public void StopGame()
        {
            if (IsGameRunning)
                GameCancellationSource.Cancel();
        }

        public bool DealStreet()
        {
            if (Players.FindAll(p => p.SitOut == false).Count < 2) return false;

            if (StreetCount < Streets.Count)
            {
                Streets[StreetCount].DealCards();
                StreetCount++;

                return true;
            }
            StreetCount = 0;

            return false;
        }

        public virtual void DealHand()
        {
            if (Players.FindAll(p => p.SitOut == false).Count < 2) return;

            StreetCount = 0;
            while (DealStreet())
            {
                StartPlayerAction();
                CollectBets();
                StreetCount++;
            }

            StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
        }

        public virtual void StartPlayerAction()
        {
            do
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == ActionSeatPosition);
                SetOptions(playerIndex);
                Players[playerIndex].Countdown = PlayerTimeout;
                while (Players[playerIndex].CurrentAction == PlayerAction.None && Players[playerIndex].Countdown > 0)
                {
                    Thread.Sleep(1000);
                    Players[playerIndex].Countdown--;
                }

                Players[playerIndex].Chips -= Players[playerIndex].Bet;

                var currentAction = Players[playerIndex].CurrentAction;
                if (currentAction == PlayerAction.Bet)
                {
                    var bet = Players[playerIndex].Bet;
                    MinBet = (bet * 2) - LastBet;
                    LastBet = bet;
                }
                else if (Players[playerIndex].Countdown == 0)
                {
                    Players[playerIndex].SitOut = true;
                    foreach (var option in Players[playerIndex].Options.AllowedActions)
                        if (option == PlayerAction.Check)
                            currentAction = PlayerAction.Check;
                    if (currentAction != PlayerAction.Check)
                        currentAction = PlayerAction.Fold;
                    Players[playerIndex].CurrentAction = currentAction;
                }
            }
            while (IncrementActionSeat());
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false && p.CurrentAction != PlayerAction.Fold);
            var activeSeat = seatNumber;
            var playerCount = 0;
            while (playerCount < activePlayers.Count)
            {
                activeSeat = GetNextSeatNumber(activeSeat, activePlayers.Count);
                if (activePlayers.Exists(p => p.SeatNumber == activeSeat))
                    return activeSeat;

                playerCount++;
            }

            return -1;
        }

        protected static int GetNextSeatNumber(int startSeat, int seats)
        {
            var newSeat = startSeat;
            newSeat++;
            if (newSeat > seats)
                newSeat = 1;
            return newSeat;
        }

        protected virtual void SetOptionsCheck(int playerIndex)
        {
            Players[playerIndex].Options.AllowedActions = new PlayerAction[]
            {
                PlayerAction.Bet,
                PlayerAction.Check,
                PlayerAction.Fold
            };
        }

        protected bool IncrementActionSeat()
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false && p.CurrentAction != PlayerAction.Fold);
            var playerCount = 0;
            while (playerCount < activePlayers.Count)
            {
                ActionSeatPosition = GetNextSeatNumber(ActionSeatPosition, Seats);
                if (activePlayers.Exists(p => p.SeatNumber == ActionSeatPosition))
                    return true;

                playerCount++;
            }

            return false;
        }

        public void DealPlayerCards(StreetBase street)
        {
            Street = street.Name;
            int cardsDealt = 0;
            while (cardsDealt < street.NumberOfCards)
            {
                var seatNumber = StartDealingSeatNumber;
                int seatCount = 0;
                while (seatCount < Seats)
                {
                    var playerIndex = Players.FindIndex(0, p => p.SeatNumber == seatNumber);
                    if (playerIndex >= 0)
                    {
                        var card = Deck.GetRandomCard();
                        card.IsHidden = street.IsHidden;
                        Players[playerIndex].Cards.Add(card);
                    }

                    seatCount++;
                    seatNumber++;
                    if (seatNumber > Seats)
                        seatNumber = 1;
                }

                cardsDealt++;
            }
        }

        public bool CollectBets()
        {
            var activePlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var activePlayer in activePlayers)
            {
                Pot += activePlayer.Bet;
                var playerIndex = Players.FindIndex(p => p.SeatNumber == activePlayer.SeatNumber);
                Players[playerIndex].Bet = 0;
                Players[playerIndex].CurrentAction = PlayerAction.None;
            }
            if (activePlayers.Count > 1)
                return true;
            return false;
        }

        private bool GetGameStatus()
        {
            if (RunningGame != null && (RunningGame.Status == TaskStatus.Running || RunningGame.Status == TaskStatus.Created))
                return true;
            return false;
        }

        private void SetOptions(int playerIndex)
        {
            if (LastBet > Players[playerIndex].Bet)
                SetOptionsCall(playerIndex);
            else
                SetOptionsCheck(playerIndex);
        }

        private void SetOptionsCall(int playerIndex)
        {
            Players[playerIndex].Options.MinBet = MinBet;
            Players[playerIndex].Options.AllowedActions = new PlayerAction[]
            {
                PlayerAction.Bet,
                PlayerAction.Call,
                PlayerAction.Fold
            };
        }
    }
}
