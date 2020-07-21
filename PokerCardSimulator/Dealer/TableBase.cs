using System;
using System.Collections.Generic;
using System.Linq;
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
        public abstract decimal Pot { get; set; }
        public abstract Streets Streets { get; set;}       
        public abstract int PlayerTimeout { get; set; }
        public bool IsGameRunning => GetGameStatus();
        public abstract List<Player> Players { get; }
        public abstract int StartDealingSeatNumber { get; set; }
        public abstract Task RunningGame { get; set; }
        public abstract Deck Deck { get; set; }
        public abstract decimal LastBet { get; set; }
        public abstract CancellationTokenSource GameCancellationSource { get; set; }

        private int ActionSeatPosition { get; set; }
        private decimal MinBet { get; set; }

        protected abstract TableViewBase GetTableView(int playerId);

        public Player GetPlayer(int seatNumber)
        {
            if (!Players.Exists(p => p.SeatNumber == seatNumber)) return null;
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            return Players[playerIndex];
        }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        public void UpdatePlayer(Player player)
        {
            if (Players.Exists(p => p.SeatNumber == player.SeatNumber))
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == player.SeatNumber);
                Players[playerIndex] = player;
            }
        }
        
        public List<Player> GetSittingPlayers()
        {
            return Players.FindAll(p => p.SitOut == false);
        }

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

        public async Task<TableViewBase> Subscribe(int playerId, CancellationToken token)
        {
            if (!Players.Exists(p => p.Id == playerId)) return null;

            var player = Players.Single(p => p.Id == playerId);
            TableViewBase tableView = null;

            player.WaitForTurn += (s, e) =>
            {
                var table = (TableBase)s;
                tableView = table.GetTableView(player.Id);
            };

            var view = await Task.Run(() =>
            {
                while (tableView == null && !token.IsCancellationRequested) { }

                return tableView;
            }, token);

            return view;
        }

        public void StartGame()
        {
            GameCancellationSource = new CancellationTokenSource();

            RunningGame = Task.Run(() =>
            {
                while (!GameCancellationSource.Token.IsCancellationRequested 
                && Players.FindAll(p => p.SitOut = false).Count >= 2)
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

            return Streets.DealCards();
        }

        public virtual void DealHand()
        {
            if (Players.FindAll(p => p.SitOut == false).Count < 2) return;

            while (DealStreet())
            {
                StartBettingRound();
                CollectBets();
            }

            StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
        }

        public virtual void StartBettingRound()
        {
            ActionSeatPosition = StartDealingSeatNumber;

            do
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == ActionSeatPosition);
                var player = GetPlayer(ActionSeatPosition);
                SetOptions(ActionSeatPosition);
                player.Countdown = PlayerTimeout;
                UpdatePlayer(player);
                NotifyPlayers();
                while (player.CurrentAction == PlayerAction.None && player.Countdown > 0)
                {
                    Thread.Sleep(1000);
                    player = GetPlayer(ActionSeatPosition);
                    player.Countdown--;
                    UpdatePlayer(player);
                }

                player.Chips -= player.Bet;

                var currentAction = player.CurrentAction;
                if (currentAction == PlayerAction.Bet)
                {
                    MinBet = (player.Bet * 2) - LastBet;
                    LastBet = player.Bet;
                }
                else if (player.Countdown == 0)
                {
                    player.SitOut = true;
                    foreach (var option in player.Options.AllowedActions)
                        if (option == PlayerAction.Check)
                            currentAction = PlayerAction.Check;
                    if (currentAction != PlayerAction.Check)
                        currentAction = PlayerAction.Fold;
                    player.CurrentAction = currentAction;
                    UpdatePlayer(player);
                }
            }
            while (IncrementActionSeat());
        }

        public void NotifyPlayers()
        {
            foreach (var player in Players)
                player.Notify(this);
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false && p.CurrentAction != PlayerAction.Fold);
            var activeSeat = GetNextSeatNumber(seatNumber, Seats);

            while (!activePlayers.Exists(p => p.SeatNumber == activeSeat))
                activeSeat = GetNextSeatNumber(activeSeat, activePlayers.Count);

            return activeSeat;
        }

        protected static int GetNextSeatNumber(int startSeat, int seats)
        {
            var newSeat = startSeat;
            newSeat++;
            if (newSeat > seats)
                newSeat = 1;
            return newSeat;
        }

        protected virtual void SetOptionsCheck(int seatNumber)
        {
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            Players[playerIndex].Options = new PlayerOptions()
            {
                AllowedActions = new PlayerAction[]
                {
                    PlayerAction.Bet,
                    PlayerAction.Check,
                    PlayerAction.Fold
                }
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

        private bool CollectBets()
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

        private void SetOptions(int seatNumber)
        {
            if (LastBet > GetPlayer(seatNumber).Bet)
                SetOptionsCall(seatNumber);
            else
                SetOptionsCheck(seatNumber);
        }

        private void SetOptionsCall(int seatNumber)
        {
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
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
