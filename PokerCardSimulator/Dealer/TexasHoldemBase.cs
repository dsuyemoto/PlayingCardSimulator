using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public abstract class TexasHoldemBase : TableBase
    {
        public abstract decimal SmallBlind { get; set; }
        public abstract decimal BigBlind { get; set; }
        public abstract int DealerButtonSeatNumber { get; set; }
        public abstract int SmallBlindSeatNumber { get; set; }
        public abstract int BigBlindSeatNumber { get; set; }
        public abstract List<Card> Community { get; set; }

        protected void InitializeStreets()
        {
            Streets.Add(new PlayerStreet(this, 2, true, StreetName.PreFlop));
            Streets.Add(new CommunityStreet(this, 3, false, StreetName.Flop));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.Turn));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.River));
        }

        public override void SitIn(int seatNumber)
        {
            if (!BetweenBlinds(seatNumber) && GetPlayer(seatNumber) != null)
                base.SitIn(seatNumber);
        }

        public override void DealHand()
        {
            if (GetSittingPlayers().Count < 2) return;

            SetBlinds();
            base.DealHand();
            MoveButton();
        }
         
        public override void StartBettingRound()
        {
            StartDealingSeatNumber = DealerButtonSeatNumber;

            if (GetSittingPlayers().Count > 2)
                StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);

            if (Streets.CurrentStreet == StreetName.PreFlop)
            {
                if (SmallBlindSeatNumber > 0)
                    StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
                if (BigBlindSeatNumber > 0)
                    StartDealingSeatNumber = GetNextActiveSeat(StartDealingSeatNumber);
            }

            base.StartBettingRound();
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

        protected override void SetOptionsCheck(int seatNumber)
        {
            var player = GetPlayer(seatNumber);
            player.Options.MinBet = BigBlind;
            UpdatePlayer(player);
            base.SetOptionsCheck(seatNumber);
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
            if (GetSittingPlayers().Count < 2) return;

            if (GetSittingPlayers().Count == 2)
            {
                if (GetPlayer(DealerButtonSeatNumber) == null)
                {
                    DealerButtonSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
                    SmallBlindSeatNumber = DealerButtonSeatNumber;
                    BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                }

                SmallBlindSeatNumber = DealerButtonSeatNumber;
                BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
            }
            else
            {
                BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                SmallBlindSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
            }

            SetBlindBet(SmallBlind, SmallBlindSeatNumber);
            SetBlindBet(BigBlind, BigBlindSeatNumber);
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
