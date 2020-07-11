using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public abstract class TexasHoldemBase : TableBase
    {
        protected double _smallBlind;
        protected double _bigBlind;

        public abstract int DealerButtonSeatNumber { get; set; }
        public abstract int SmallBlindSeatNumber { get; set; }
        public abstract int BigBlindSeatNumber { get; set; }
        public abstract List<Card> Community { get; set; }

        public TexasHoldemBase()
        {
            Streets.Add(new PlayerStreet(this, 2, true, StreetName.PreFlop));
            Streets.Add(new CommunityStreet(this, 3, false, StreetName.Flop));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.Turn));
            Streets.Add(new CommunityStreet(this, 1, false, StreetName.River));
            Street = StreetName.PreFlop;
        }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            var playerSeated = base.SeatPlayer(player, seatNumber);
            if (Players.FindAll(p => p.SitOut == false).Count == 1)
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
                DealerButtonSeatNumber = Players[playerIndex].SeatNumber;
            }

            return playerSeated; 
        }

        public override void SitIn(int seatNumber)
        {
            if (!BetweenBlinds(seatNumber) && Players.Exists(p => p.SeatNumber == seatNumber))
            {
                base.SitIn(seatNumber);
            }
        }

        public override void DealHand()
        {
            if (Players.FindAll(p => p.SitOut == false).Count < 2) return;

            SetBlinds();
            base.DealHand();
            MoveButton();
        }

        public override void StartPlayerAction()
        {
            ActionSeatPosition = StartDealingSeatNumber;

            if (Players.FindAll(p => p.SitOut == false).Count > 2)
            {
                if (_smallBlind > 0)
                    IncrementActionSeat();
                if (_bigBlind > 0)
                    IncrementActionSeat();
            }

            base.StartPlayerAction();
        }

        public virtual TexasHoldemView GetTableView(int playerId)
        {
            return new TexasHoldemView(this, playerId);
        }

        public void DealCommunityCards(StreetBase street)
        {
            Street = street.Name;
            var cardCount = 0;
            while (cardCount < street.NumberOfCards)
            {
                var card = Deck.GetRandomCard();
                card.IsHidden = street.IsHidden;
                Community.Add(card);
                cardCount++;
            }
        }

        protected override void SetOptionsCheck(int playerIndex)
        {
            Players[playerIndex].Options.MinBet = _bigBlind;
            base.SetOptionsCheck(playerIndex);
        }

        protected virtual void SetBlindBets(double blind, int seatNumber)
        {
            if (Players.Exists(p => p.SeatNumber == seatNumber))
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == seatNumber);
                Players[playerIndex].Bet = blind;
                Players[playerIndex].Chips -= blind;
                LastBet = blind;
            }
        }

        private void SetBlinds()
        {
            if (Players.FindAll(p => p.SitOut == false).Count < 2) return;

            if (Players.FindAll(p => p.SitOut == false).Count == 2)
            {
                if (Players.Exists(p => p.SeatNumber == DealerButtonSeatNumber))
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
                if (Players.Exists(p => p.SeatNumber == DealerButtonSeatNumber))
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

            SetBlindBets(_smallBlind, SmallBlindSeatNumber);
            SetBlindBets(_bigBlind, BigBlindSeatNumber);
        }

        private void MoveButton()
        {
            if (Players.Count > 2)
            {
                if (Players.Exists(p => p.SeatNumber == SmallBlindSeatNumber))
                    DealerButtonSeatNumber = SmallBlindSeatNumber;
                if (Players.Exists(p => p.SeatNumber == BigBlindSeatNumber))
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
            if (!Players.Exists(p => p.SeatNumber == DealerButtonSeatNumber)) return false;

            var activePlayers = Players.FindAll(p => p.SitOut == false);
            var orderedPlayers = activePlayers.OrderBy(p => p.SeatNumber).ToList();
            var playersOrderedByButton = new List<Player>();
            var startingSeatNumber = DealerButtonSeatNumber;
            for (var i =0;i < orderedPlayers.Count; i++)
            {
                var playerIndex = Players.FindIndex(p => p.SeatNumber == startingSeatNumber);
                if (playerIndex > 0)
                {
                    playersOrderedByButton.Add(Players[playerIndex]);
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
