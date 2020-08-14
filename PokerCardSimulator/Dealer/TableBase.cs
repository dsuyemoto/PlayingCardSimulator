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
        public abstract double PlayerTimeoutMilliseconds { get; set; }
        public bool IsGameRunning { get; set; }
        public abstract List<Player> Players { get; }
        public abstract int StartDealingSeatNumber { get; set; }
        public abstract Deck Deck { get; set; }
        public abstract decimal LastBet { get; set; }
        public abstract int DealerButtonSeatNumber { get; set; }
        public event EventHandler GameStarted;

        private decimal MinBet { get; set; }

        protected abstract TableViewBase GetTableView(int playerId);

        public TableBase()
        {
            GameStarted += GameStartedHandler;
        }

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
            if (!Players.Exists(p => p.SittingOut == false && p.SeatNumber == seatNumber)) return null;
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            return Players[playerIndex];
        }

        public List<Player> GetActivePlayers()
        {
            return Players.FindAll(p => p.SittingOut == false && p.CurrentAction != PlayerAction.Fold);
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
            return Players.FindAll(p => p.SittingOut == false);
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (Players.Exists((p) => p.SeatNumber == seatNumber)) return false;

            player.SeatNumber = seatNumber;
            player.SittingOut = true;
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
                Players[playerIndex].SittingOut = true;
            }
        }

        public virtual void SitIn(int seatNumber)
        {
            var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
            Players[playerIndex].SittingOut = false;
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
            IsGameRunning = true;
            onGameStarted(this, EventArgs.Empty);
        }

        public void StopGame()
        {
            IsGameRunning = false;
        }

        public virtual void DealHand()
        {
            if (GetActivePlayers().Count < 2) return;

            do
            {
                Streets.DealCards();
                Streets.StartBettingRound(DealerButtonSeatNumber);
                Streets.CollectBets();
            }
            while (Streets.Next() && GetActivePlayers().Count > 1);

            Streets.PayWinner();

            StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
        }

        public virtual void StartBettingRound(int dealerButtonSeatNumber)
        {
            var firstPlayer = GetNextActivePlayer(dealerButtonSeatNumber);
            var player = firstPlayer;

            do
            {
                if (GetActivePlayers().Count == 1) break;

                player.CurrentAction = PlayerAction.None;
                UpdatePlayer(player);
                player.OnActionPrompted(this, new ActionPromptedEventArgs() { 
                    PlayerOptions = GetOptions(player) 
                });
                GetPlayerAction(player);
                player = GetNextActivePlayer(player.SeatNumber);
            }
            while (player != null && (player.SeatNumber != firstPlayer.SeatNumber || player.Bet != LastBet));
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

        public void CollectBets()
        {
            var betPlayers = Players.FindAll(s => s.Bet > 0);
            foreach (var betPlayer in betPlayers)
            {
                Pot += betPlayer.Bet;
                var playerIndex = Players.FindIndex(p => p.SeatNumber == betPlayer.SeatNumber);
                Players[playerIndex].Bet = 0;
            }
        }

        public void PayWinner()
        {
            player.Chips += Pot;
            Pot = 0;
        }

        protected virtual Player GetNextActivePlayer(int activeSeatNumber)
        {
            activeSeatNumber = GetNextActiveSeat(activeSeatNumber); 
            
            return GetPlayer(activeSeatNumber);
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            var activePlayers = Players.FindAll(p => p.SittingOut == false && p.CurrentAction != PlayerAction.Fold);
            if (activePlayers.Count == 0) return -1;
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

        private PlayerOptions GetOptions(Player player)
        {
            if (LastBet > player.Bet)
                return GetOptionsCall();
            else
                return GetOptionsCheck();
        }

        private void GetPlayerAction(Player player)
        {
            player.Timer = new System.Timers.Timer(PlayerTimeoutMilliseconds);
            player.Timer.Elapsed += (s, e) => PlayerTimedOut(player);
            player.Timer.Enabled = true;

            while (GetPlayer(player).CurrentAction == PlayerAction.None) { }

            UpdatePlayerBet(GetPlayer(player));
        }

        private void PlayerTimedOut(Player player)
        {
            player.SittingOut = true;
            player.CurrentAction = PlayerAction.Fold;
            var playerOptions = GetOptions(player);
            if (playerOptions.AllowedActions.Contains(PlayerAction.Check))
                player.CurrentAction = PlayerAction.Check;

            UpdatePlayer(player);
        }

        private void UpdatePlayerBet(Player player)
        {
            if (player.CurrentAction == PlayerAction.Bet)
            {
                MinBet = (player.Bet * 2) - LastBet;
                LastBet = player.Bet;
            }

            UpdatePlayer(player);
        }

        private void onGameStarted(object sender, EventArgs eventArgs)
        {
            GameStarted?.Invoke(sender, eventArgs);
        }

        private void GameStartedHandler(object sender, EventArgs eventArgs)
        {
            RunGame();
        }

        private void RunGame()
        {
            DealHand();
            if (Players.FindAll(p => p.SittingOut = false).Count >= 2 && IsGameRunning)
                RunGame();
            else
                IsGameRunning = false;
        }
    }
}
