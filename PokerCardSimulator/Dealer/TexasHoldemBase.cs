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

        public void InitializeStreets()
        {
            Streets.Add(new PlayerStreet(this, 2, true, StreetName.PreFlop));
            Streets.Add(new CommunityStreet(this, 3, false, StreetName.Flop));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.Turn));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.River));
        }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            var playerSeated = base.SeatPlayer(player, seatNumber);

            if (GetSittingPlayers().Count == 1)
                DealerButtonSeatNumber = seatNumber;

            return playerSeated; 
        }

        public override void SitIn(int seatNumber)
        {
            if (!BetweenBlinds(seatNumber) && GetPlayer(seatNumber) != null)
            {
                base.SitIn(seatNumber);
            }
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
            if (GetSittingPlayers().Count > 2)
            {
                if (SmallBlind > 0)
                    IncrementActionSeat();
                if (BigBlind > 0)
                    IncrementActionSeat();
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

        protected virtual void SetBlindBets(decimal blind, int seatNumber)
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
                if (GetPlayer(DealerButtonSeatNumber) != null)
                {
                    if (SmallBlindSeatNumber == 0)
                        SmallBlindSeatNumber = DealerButtonSeatNumber;
                    if (BigBlindSeatNumber == 0)
                        BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                }
                else
                {
                    SmallBlindSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
                    BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                    DealerButtonSeatNumber = SmallBlindSeatNumber;
                }             
            }
            else
            {
                if (GetPlayer(DealerButtonSeatNumber) != null)
                {
                    if (SmallBlindSeatNumber == 0)
                        SmallBlindSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
                    if (BigBlindSeatNumber == 0)
                        BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                }
                else
                {
                    DealerButtonSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
                    SmallBlindSeatNumber = GetNextActiveSeat(DealerButtonSeatNumber);
                    BigBlindSeatNumber = GetNextActiveSeat(SmallBlindSeatNumber);
                }
            }

            SetBlindBets(SmallBlind, SmallBlindSeatNumber);
            SetBlindBets(BigBlind, BigBlindSeatNumber);
        }

        private void MoveButton()
        {
            if (GetPlayers().Count > 2)
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
            var startingSeatNumber = DealerButtonSeatNumber;
            for (var i =0;i < orderedPlayers.Count; i++)
            {
                var player = GetPlayer(startingSeatNumber);
                if (player != null)
                {
                    playersOrderedByButton.Add(player);
                    startingSeatNumber = GetNextActiveSeat(startingSeatNumber);
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
