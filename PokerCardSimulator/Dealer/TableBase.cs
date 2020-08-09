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
            River,
            Ended
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
        public abstract int DealerButtonSeatNumber { get; set; }

        private int ActionSeatPosition { get; set; }
        private decimal MinBet { get; set; }

        protected abstract TableViewBase GetTableView(int playerId);

        public Player GetPlayer(Player player)
        {
            return GetPlayer(player.SeatNumber);
        }

        public Player GetPlayer(int seatNumber)
        {
            if (!Players.Exists(p => p.SeatNumber == seatNumber)) return null;
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            return Players[playerIndex];
        }

        protected Player GetActivePlayer(int seatNumber)
        {
            if (!Players.Exists(p => p.SitOut == false && p.SeatNumber == seatNumber)) return null;
            var playerIndex = Players.FindIndex(p => p.SitOut == false && p.SeatNumber == seatNumber);
            return Players[playerIndex];
        }

        public List<Player> GetActivePlayers()
        {
            return Players.FindAll(p => p.SitOut == false && p.CurrentAction != PlayerAction.Fold);
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

        public bool SeatPlayer(Player player, int seatNumber)
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

            player.ActionPrompted += (s, e) =>
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

        public virtual void DealHand()
        {
            if (GetActivePlayers().Count < 2) return;

            do
            {
                Streets.DealCards();
                Streets.StartBettingRound(DealerButtonSeatNumber);
                CollectBets();
            }
            while (Streets.Next());

            StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
        }

        public virtual void StartBettingRound(int dealerButtonSeatNumber)
        {
            var firstPlayer = GetNextActivePlayer(dealerButtonSeatNumber);
            var player = firstPlayer;

            do
            {
                player.CurrentAction = PlayerAction.None;
                UpdatePlayer(player);
                player.OnActionPrompted(this, new ActionPromptedEventArgs() { 
                    PlayerOptions = GetOptions(player) 
                });
                GetPlayerAction(player);
                player = GetNextActivePlayer(player.SeatNumber);
            }
            while (player.SeatNumber != firstPlayer.SeatNumber || player.Bet != LastBet);
        }

        private void GetPlayerAction(Player player)
        {
            player.Timer = new System.Timers.Timer(PlayerTimeout * 1000);
            player.Timer.Elapsed += (s, e) => PlayerTimedOut(player);
            player.Timer.Enabled = true;

            while (GetPlayer(player).CurrentAction == PlayerAction.None) { }

            UpdatePlayerBet(GetPlayer(player.SeatNumber));
        }

        private void PlayerTimedOut(Player player)
        {
            player.SitOut = true;
            player.CurrentAction = PlayerAction.Fold;
            var playerOptions = GetOptions(player);
            if (playerOptions.AllowedActions.Contains(PlayerAction.Check))
                player.CurrentAction = PlayerAction.Check;

            UpdatePlayer(player);
        }

        private void UpdatePlayerBet(Player player)
        {
            foreach (var player in Players)
                player.Notify(this);
        }

            UpdatePlayer(player);
        }

        protected virtual Player GetNextActivePlayer(int activeSeatNumber)
        {
            activeSeatNumber = GetNextActiveSeat(activeSeatNumber); 
            
            return GetPlayer(activeSeatNumber);
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false && p.CurrentAction != PlayerAction.Fold);
            var activeSeat = GetNextSeatNumber(seatNumber);

            while (!activePlayers.Exists(p => p.SeatNumber == activeSeat))
                activeSeat = GetNextSeatNumber(activeSeat);

            return activeSeat;
        }

        protected int GetNextSeatNumber(int startSeat)
        {
            var newSeat = startSeat;
            newSeat++;
            if (newSeat > Seats)
                newSeat = 1;
            return newSeat;
        }

        protected virtual PlayerOptions GetOptionsCheck()
        {
            return new PlayerOptions()
            {
                MinBet = MinBet,
                AllowedActions = new PlayerAction[]
                {
                    PlayerAction.Bet,
                    PlayerAction.Check,
                    PlayerAction.Fold
                }           
            };
        }

        protected virtual PlayerOptions GetOptionsCall()
        {
            return new PlayerOptions()
            {
                MinBet = MinBet,
                AllowedActions = new PlayerAction[]
                {
                    PlayerAction.Bet,
                    PlayerAction.Call,
                    PlayerAction.Fold
                }
            };
        }

        public void DealPlayerCards(StreetBase street)
        {
            int cardsDealt = 0;
            while (cardsDealt < street.NumberOfCards)
            {
                var activePlayers = GetActivePlayers();
                var seatNumber = StartDealingSeatNumber;
                int seatCount = 0;
                while (seatCount < activePlayers.Count)
                {
                    var playerIndex = activePlayers.FindIndex(0, p => p.SeatNumber == seatNumber);
                    if (playerIndex > -1)
                    {
                        var card = Deck.GetRandomCard();
                        card.IsHidden = street.IsHidden;
                        Players[playerIndex].Cards.Add(card);
                    }

                    seatCount++;
                    seatNumber = GetNextSeatNumber(seatNumber);
                }

                cardsDealt++;
            }
        }

        private void CollectBets()
        {
            var betPlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var betPlayer in betPlayers)
            {
                Pot += betPlayer.Bet;
                var playerIndex = Players.FindIndex(p => p.SeatNumber == betPlayer.SeatNumber);
                Players[playerIndex].Bet = 0;
            }
        }

        private bool GetGameStatus()
        {
            if (RunningGame != null && (RunningGame.Status == TaskStatus.Running || RunningGame.Status == TaskStatus.Created))
                return true;
            return false;
        }

        private PlayerOptions GetOptions(Player player)
        {
            if (LastBet > player.Bet)
                return GetOptionsCall();
            else
                return GetOptionsCheck();
        }
    }
}
