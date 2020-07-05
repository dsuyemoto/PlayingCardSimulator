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
        public abstract int PlayerTimeout { get; set; }
        public bool IsGameRunning => GetGameStatus();
        public abstract List<Player> Players { get; set; }
        public abstract int ActionSeatPosition { get; set; }
        public abstract int StartDealingAtSeatNumber { get; set; }
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
                StartDealingAtSeatNumber = seatNumber;

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

        public abstract bool Deal();

        public virtual void DealHand()
        {   
            bool dealing = true;
            while (dealing)
            {
                dealing = Deal();
                StartPlayerAction();
                CollectBets();
            }

            GetNextActiveSeat(StartDealingAtSeatNumber);
        }

        public virtual void StartPlayerAction()
        {
            int playersActed = 0;
            
            var playersToAct = Players.FindAll(p => p.SitOut = false && p.LastAction != PlayerAction.Fold);

            while (playersActed < playersToAct.Count)
            {
                if (Players.Exists(p => p.SeatNumber == ActionSeatPosition))
                {
                    var playerIndex = Players.FindIndex(p => p.SeatNumber == ActionSeatPosition);
                    SetOptions(playerIndex);
                    var currentAction = Players[playerIndex].CurrentAction;
                    int timeoutCount = 0;
                    while (currentAction == PlayerAction.None && timeoutCount < PlayerTimeout)
                    {
                        currentAction = Players[playerIndex].CurrentAction;
                        Thread.Sleep(1000);
                        timeoutCount++;
                    }

                    if (currentAction == PlayerAction.Call)
                    {
                        Players[playerIndex].Bet = LastBet;
                    }
                    else if (currentAction == PlayerAction.Bet)
                    {
                        var bet = Players[playerIndex].Bet;
                        MinBet = (bet * 2) - LastBet;
                        LastBet = bet;
                    }
                    else if (timeoutCount >= PlayerTimeout)
                    {
                        Players[playerIndex].SitOut = true;
                        foreach (var option in Players[playerIndex].Options.AllowedActions)
                            if (option == PlayerAction.Check)
                                currentAction = PlayerAction.Check;
                        if (currentAction != PlayerAction.Check)
                            currentAction = PlayerAction.Fold;
                    }

                    Players[playerIndex].LastAction = currentAction;
                    Players[playerIndex].CurrentAction = PlayerAction.None;

                    playersActed++;
                }
                else
                {
                    IncrementActionSeat();
                }
            }
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            var seatCount = seatNumber;
            var playerCount = 0;
            while (playerCount < Seats)
            {
                if (Players.Exists(p => p.SeatNumber == seatCount && p.SitOut == false))
                    return seatCount;
                else
                    seatCount = GetNextSeatNumber(seatCount, Seats);
                playerCount++;
            }

            return seatNumber;
        }

        protected int GetNextPlayer(int seatNumber)
        {
            var seatCount = seatNumber;
            var playerCount = 0;
            while (playerCount < Seats)
            {
                if (Players.Exists(p => p.SeatNumber == seatCount))
                    return seatCount;
                else
                    seatCount = GetNextSeatNumber(seatCount, Seats);
                playerCount++;
            }

            return seatNumber;
        }

        protected static int GetNextSeatNumber(int startSeat, int seats)
        {
            var newSeat = startSeat;
            newSeat++;
            if (newSeat > seats)
                newSeat = 1;
            return newSeat;
        }

        protected void DealHoleCards(int number)
        {
            var dealtCards = 0;
            while (dealtCards < number)
            {
                DealPlayerCards(true);
                dealtCards++;
            }
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

        protected void IncrementActionSeat()
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false && p.LastAction != PlayerAction.Fold) as List<Player>;
            int seatCount = 0;
            do
            {
                ActionSeatPosition++;
                if (ActionSeatPosition > Seats)
                    ActionSeatPosition = 1;
                seatCount++;
            }
            while (!activePlayers.Exists(p => p.SeatNumber == ActionSeatPosition && seatCount < Seats));
        }

        private void DealPlayerCards(bool isHidden)
        {
            var seatNumber = StartDealingAtSeatNumber;
            int seatCount = 0;
            while (seatCount < Seats)
            {
                var playerIndex = Players.FindIndex(0, p => p.SeatNumber == seatNumber);
                if (playerIndex >= 0)
                {
                    var card = Deck.GetRandomCard();
                    card.IsHidden = isHidden;
                    Players[playerIndex].Cards.Add(card);
                }

                seatCount++;
                seatNumber++;
                if (seatNumber > Seats)
                    seatNumber = 1;
            }
        }

        private bool CollectBets()
        {
            var activePlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var activePlayer in activePlayers)
                Pot += activePlayer.Bet;
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
