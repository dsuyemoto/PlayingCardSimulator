using System.Collections.Generic;
using System.Linq;
using static Dealer.Player;

namespace Dealer
{
    public abstract class TexasHoldemBase : TableBase
    {
        public abstract decimal SmallBlind { get; set; }
        public abstract decimal BigBlind { get; set; }
        public abstract int SmallBlindSeatNumber { get; set; }
        public abstract int BigBlindSeatNumber { get; set; }
        public abstract List<Card> Community { get; set; }

        private Dictionary<BlindName, Player> Blinds { get; set; } = new Dictionary<BlindName, Player>();

        protected void InitializeStreets()
        {
            Streets.Add(new TexasHoldemPlayerStreet(this, 2, true, StreetName.PreFlop));
            Streets.Add(new TexasHoldemCommunityStreet(this, 3, false, StreetName.Flop));
            Streets.Add(new TexasHoldemCommunityStreet(this, 1, false, StreetName.Turn));
            Streets.Add(new TexasHoldemCommunityStreet(this, 1, false, StreetName.River));
        }

        public override void SitIn(int seatNumber)
        {
            if (!BetweenBlinds(seatNumber) && GetPlayer(seatNumber) != null)
                base.SitIn(seatNumber);
        }

        public override void DealHand()
        {
            if (GetActivePlayers().Count < 2) return;

            base.DealHand();
            MoveButton();
        }
         
        public override void StartBettingRound(int startingSeatNumber)
        {
            var startDealingSeatNumber = startingSeatNumber;

            if (GetActivePlayers().Count > 2)
                startDealingSeatNumber = GetNextActiveSeat(startDealingSeatNumber);

            if (Streets.CurrentStreet == StreetName.PreFlop)
            {
                if (SmallBlindSeatNumber > 0)
                    startDealingSeatNumber = GetNextActiveSeat(startDealingSeatNumber);
                if (BigBlindSeatNumber > 0)
                    startDealingSeatNumber = GetNextActiveSeat(startDealingSeatNumber);
            }

            base.StartBettingRound(startDealingSeatNumber);
        }

        public void DealCommunityCards(StreetBase street)
        {
            var cardCount = 0;
            while (cardCount < street.NumberOfCards)
            {
                var card = Deck.GetRandomCard();
                card.IsHidden = street.IsHidden;
                Community.Add(card);
                cardCount++;
            }
        }

        protected override PlayerOptions GetOptionsCheck()
        {
            var playerOptions = base.GetOptionsCheck();
            playerOptions.MinBet = BigBlind;

            return playerOptions;
        }

        protected virtual void SetBlindBet(decimal blind, int seatNumber)
        {
            var player = GetPlayer(seatNumber);
            if (player != null)
            {
                player.Bet = blind;
                player.Chips -= blind;
                UpdatePlayer(player);
                LastBet = blind;
            }
        }

        public void SetBlinds()
        {
            var smallBlindPlayer = GetBlindPlayer(BlindName.Small);
            var bigBlindPlayer = GetBlindPlayer(BlindName.Big);

            if (smallBlindPlayer == null && bigBlindPlayer == null)
            {
                var activePlayers = GetActivePlayers();

                if (activePlayers.Count == 2)
                    smallBlindPlayer = activePlayers.Single(p => p.SeatNumber == DealerButtonSeatNumber);
                else
                    smallBlindPlayer = GetNextActivePlayer(DealerButtonSeatNumber);

                bigBlindPlayer = GetNextActivePlayer(smallBlindPlayer.SeatNumber);
            }
            if (smallBlindPlayer != null)
            {
                SetBlindPlayer(BlindName.Small, smallBlindPlayer);
                SetBlindBet(SmallBlind, smallBlindPlayer.SeatNumber);
            }
            if (bigBlindPlayer != null)
            {
                SetBlindPlayer(BlindName.Big, bigBlindPlayer);
                SetBlindBet(BigBlind, bigBlindPlayer.SeatNumber);
            }
        }

        public Player GetBlindPlayer(BlindName blindName)
        {
            if (Blinds.ContainsKey(blindName))
                return Blinds[blindName];

            return null;
        }

        private void SetBlindPlayer(BlindName blindName, Player player)
        {
            if (Blinds.ContainsKey(blindName))
                Blinds[blindName] = player;
            else
                Blinds.Add(blindName, player);
        }

        public void FixDealerButton()
        {
            if (GetActivePlayers().Count == 2)
            {
                var smallBlindPlayer = GetBlindPlayer(BlindName.Small);
                if (GetBlindPlayer(BlindName.Small) == null)
                    smallBlindPlayer = GetNextActivePlayer(DealerButtonSeatNumber);

                DealerButtonSeatNumber = smallBlindPlayer.SeatNumber;
            }
        }

        private void MoveButton()
        {
            if (Players.Count > 2)
            {
                if (GetPlayer(SmallBlindSeatNumber) != null)
                    DealerButtonSeatNumber = SmallBlindSeatNumber;
                if (GetPlayer(BigBlindSeatNumber) != null)
                    SmallBlindSeatNumber = BigBlindSeatNumber;
 
                BigBlindSeatNumber = GetNextActiveSeat(BigBlindSeatNumber);
            }
            else
            {
                var tempSmallBlindSeatNumber = SmallBlindSeatNumber;
                DealerButtonSeatNumber = BigBlindSeatNumber;
                SmallBlindSeatNumber = BigBlindSeatNumber;
                BigBlindSeatNumber = tempSmallBlindSeatNumber;
            }
        }

        private bool BetweenBlinds(int seatNumber)
        {
            if (GetPlayer(DealerButtonSeatNumber) == null) return false;

            var activePlayers = GetSittingPlayers();
            var orderedPlayers = activePlayers.OrderBy(p => p.SeatNumber).ToList();
            var playersOrderedByButton = new List<Player>();
            var nextSeatNumber = DealerButtonSeatNumber;
            for (var i =0;i < orderedPlayers.Count; i++)
            {
                var player = GetPlayer(nextSeatNumber);
                if (player != null)
                {
                    playersOrderedByButton.Add(player);
                    nextSeatNumber = GetNextActiveSeat(nextSeatNumber);
                }
            }

            var dealerButtonIndex = playersOrderedByButton.FindIndex(p => p.SeatNumber == DealerButtonSeatNumber);
            var smallBlindIndex = playersOrderedByButton.FindIndex(p => p.SeatNumber == SmallBlindSeatNumber);
            var bigBlindIndex = playersOrderedByButton.FindIndex(p => p.SeatNumber == BigBlindSeatNumber);

            if (dealerButtonIndex < seatNumber && seatNumber <  smallBlindIndex || 
                smallBlindIndex < seatNumber && seatNumber < bigBlindIndex)
            {
                return true;
            }

            return false;
        }
    }
}
