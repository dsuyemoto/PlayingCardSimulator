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
        private Player _firstActivePlayer;
        private Player _activePlayer;
        private List<Player> _players = new List<Player>();
        private decimal _minBet;

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
        public abstract Streets Streets { get; set; }
        public abstract double PlayerTimeoutMilliseconds { get; set; }
        public bool IsGameRunning { get; set; }
        public abstract int StartDealingSeatNumber { get; set; }
        public abstract Deck Deck { get; set; }
        public abstract decimal LastBet { get; set; }
        public abstract int DealerButtonSeatNumber { get; set; }
        public event EventHandler GameStarted;

        public TableBase()
        {
            GameStarted += GameStartedHandler;
            InitializeStreets();
        }

        public abstract void InitializeStreets();
        public abstract void PayWinner();
        protected abstract TableViewBase GetTableView(int playerId);

        public Player GetPlayer(Player player)
        {
            return GetPlayer(player.SeatNumber);
        }

        public Player GetPlayer(int seatNumber)
        {
            if (!_players.Exists(p => p.SeatNumber == seatNumber)) return null;
            var playerIndex = _players.FindIndex(p => p.SeatNumber == seatNumber);
            return _players[playerIndex];
        }

        protected Player GetActivePlayer(int seatNumber)
        {
            if (!_players.Exists(p => p.SittingOut == false && p.SeatNumber == seatNumber)) return null;
            var playerIndex = _players.FindIndex(p => p.SeatNumber == seatNumber);
            return _players[playerIndex];
        }

        public List<Player> GetActivePlayers()
        {
            return _players.FindAll(p => p.SittingOut == false && p.CurrentAction != PlayerAction.Fold);
        }

        public void UpdatePlayer(Player player)
        {
            if (_players.Exists(p => p.SeatNumber == player.SeatNumber))
            {
                var playerIndex = _players.FindIndex(p => p.SeatNumber == player.SeatNumber);
                _players[playerIndex] = player;
            }
        }
        
        public List<Player> GetSittingPlayers()
        {
            return _players.FindAll(p => p.SittingOut == false);
        }

        public bool SeatPlayer(Player player, int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (_players.Exists((p) => p.SeatNumber == seatNumber)) return false;

            player.SeatNumber = seatNumber;
            player.SittingOut = true;
            _players.Add(player);

            if (_players.Count == 1)
                StartDealingSeatNumber = seatNumber;

            return true;
        }

        public virtual bool UnseatPlayer(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > Seats) throw new Exception("seat number invalid");

            if (!_players.Exists(p => p.SeatNumber == seatNumber)) return false;

            var playerIndex = _players.FindIndex(p => p.SeatNumber == seatNumber);
            _players.RemoveAt(playerIndex);

            return true;
        }

        public virtual void SitOut(int seatNumber)
        {
            if (_players.Exists(p => p.SeatNumber == seatNumber))
            {
                var playerIndex = _players.FindIndex(p => p.SeatNumber == seatNumber);
                _players[playerIndex].SittingOut = true;
            }
        }

        public virtual void SitIn(int seatNumber)
        {
            var playerIndex = _players.FindIndex(p => p.SeatNumber == seatNumber);
            _players[playerIndex].SittingOut = false;
        }

        public async Task<TableViewBase> Subscribe(int playerId, CancellationToken token)
        {
            if (!_players.Exists(p => p.Id == playerId)) return null;

            var player = _players.Single(p => p.Id == playerId);
            TableViewBase tableView = null;

            //player.ActionPrompted += (s, e) =>
            //{
            //    var table = (TableBase)s;
            //    tableView = table.GetTableView(player.Id);
            //};

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
                CollectBets();
            }
            while (Streets.Next() && GetActivePlayers().Count > 1);

            PayWinner();

            StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
        }

        public virtual void StartBettingRound(int startingSeatNumber)
        {
            var players = GetActivePlayers();
            foreach (var player in players)
            {
                player.CurrentAction = PlayerAction.None;
                UpdatePlayer(player);
            }
            _activePlayer = null;
            while (GetNextActionPlayer(startingSeatNumber))
            {
                _activePlayer.OnActionPrompted(this, new ActionPromptedEventArgs() { 
                    PlayerOptions = GetOptions(_activePlayer) 
                });
                UpdatePlayer(_activePlayer);
                GetPlayerAction(_activePlayer);
            }
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
                        _players[playerIndex].Cards.Add(card);
                    }

                    seatCount++;
                    seatNumber = GetNextSeatNumber(seatNumber);
                }

                cardsDealt++;
            }
        }

        public void CollectBets()
        {
            var betPlayers = _players.FindAll(s => s.Bet > 0);
            foreach (var betPlayer in betPlayers)
            {
                Pot += betPlayer.Bet;
                var playerIndex = _players.FindIndex(p => p.SeatNumber == betPlayer.SeatNumber);
                _players[playerIndex].Bet = 0;
            }
        }

        private bool GetNextActionPlayer(int startingSeatNumber)
        {
            if (GetActivePlayers().Count == 1)
            {
                _firstActivePlayer = null;
                return false;
            }
            if (_firstActivePlayer == null)
            {
                _firstActivePlayer = GetPlayer(GetNextActiveSeat(startingSeatNumber));
                _activePlayer = _firstActivePlayer;
            }
            else
            {
                var nextPlayer = GetPlayer(GetNextActiveSeat(_activePlayer.SeatNumber));
                if (nextPlayer == null) return false;
                if (nextPlayer.CurrentAction != PlayerAction.None && nextPlayer.Bet == LastBet)
                    return false;
                else
                    _activePlayer = nextPlayer;
            }

            return true;
        }

        protected virtual Player GetNextActivePlayer(int activeSeatNumber)
        {
            activeSeatNumber = GetNextActiveSeat(activeSeatNumber); 
            
            return GetPlayer(activeSeatNumber);
        }

        protected virtual int GetNextActiveSeat(int seatNumber)
        {
            if (GetActivePlayers().Count == 0) return -1;

            var activeSeat = seatNumber;
            do
            {
                activeSeat = GetNextSeatNumber(activeSeat);
            }
            while (GetActivePlayer(activeSeat) == null);
                

            return activeSeat;
        }

        private int GetNextSeatNumber(int startSeat)
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
                MinBet = _minBet,
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
                MinBet = _minBet,
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
            player.Timer.Dispose();
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
                _minBet = (player.Bet * 2) - LastBet;
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
            if (_players.FindAll(p => p.SittingOut = false).Count >= 2 && IsGameRunning)
                RunGame();
            else
                IsGameRunning = false;
        }
    }
}
