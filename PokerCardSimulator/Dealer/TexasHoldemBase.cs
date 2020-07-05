using System.Collections.Generic;
using System.Linq;

namespace Dealer
{
    public abstract class TexasHoldemBase : TableBase
    {
        protected double _smallBlind;
        protected double _bigBlind;

        public abstract int[] Blinds { get; set; }
        public abstract int DealerButton { get; set; }
        public override List<Player> Players { get; set; } = new List<Player>();
        public abstract List<Card> Community { get; set; }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            var playerSeated = base.SeatPlayer(player, seatNumber);
            if (Players.Count == 1)
            {
                if (!Players.Exists(p => p.SeatNumber == DealerButton))
                {
                    DealerButton = GetNextPlayer(DealerButton);
                    StartDealingAtSeatNumber = DealerButton;
                }
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

        public override bool Deal()
        {
            if (Players.Count < 2) return false;

            if (Street == StreetName.PreFlop)
            {
                DealHoleCards(2);
                Street = StreetName.Flop;

                return true;
            }
            else if (Street == StreetName.Flop)
            {
                DealCommunityCards(3);
                Street = StreetName.Turn;

                return true;
            }
            else if (Street == StreetName.Turn)
            {
                DealCommunityCards(1);
                Street = StreetName.River;

                return true;
            }
            else if (Street == StreetName.River)
            {
                DealCommunityCards(1);
                Street = StreetName.PreFlop;
            }

            return false;
        }

        public override void DealHand()
        {
            SetBlinds();

            base.DealHand();

            GetNextActiveSeat(DealerButton);
        }

        public override void StartPlayerAction()
        {
            if (Players.FindAll(p => p.SitOut == false).Count >= 2)
            {
                IncrementActionSeat();
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

        protected void DealCommunityCards(int number)
        {
            var cardCount = 0;
            while (cardCount < number)
            {
                var card = Deck.GetRandomCard();
                card.IsHidden = false;
                Community.Add(card);
                cardCount++;
            }
        }

        protected override void SetOptionsCheck(int playerIndex)
        {
            Players[playerIndex].Options.MinBet = _bigBlind;
            base.SetOptionsCheck(playerIndex);
        }

        protected virtual void SetBlind(double blind, int playerIndex)
        {
            Players[playerIndex].Bet = blind;
            Players[playerIndex].Chips -= blind;
        }

        private void SetBlinds()
        {
            if (Players.Count < 2) return;

            MoveBlinds();
            if (Players.Exists(p => p.SeatNumber == Blinds[1]))
            {
                var smallBlindIndex = Players.FindIndex(p => p.SeatNumber == Blinds[1]);
                SetBlind(_smallBlind, smallBlindIndex);
            }
            if (Players.Exists(p => p.SeatNumber == Blinds[2]))
            {
                var bigBlindIndex = Players.FindIndex(p => p.SeatNumber == Blinds[2]);
                SetBlind(_bigBlind, bigBlindIndex);
            }
        }

        private void MoveBlinds()
        {
            var activePlayers = Players.FindAll(p => p.SitOut == false).OrderBy(p => p.SeatNumber).ToList();

            if (Blinds == null)
            {
                Blinds = new int[3];
                Blinds[0] = DealerButton;
                var smallBlind = DealerButton;
                var bigBlind = GetNextActiveSeat(smallBlind);

                if (Players.Count > 2)
                {
                    smallBlind = GetNextActiveSeat(DealerButton);
                    Blinds[1] = smallBlind;
                    bigBlind = GetNextActiveSeat(smallBlind);
                    Blinds[2] = bigBlind;
                }

                Blinds[1] = smallBlind;
                Blinds[2] = bigBlind;
            }
            else
            {
                var smallBlind = Blinds[1];
                Blinds[0] = Blinds[2];
                Blinds[1] = Blinds[2];
                Blinds[2] = smallBlind;

                if (Players.Count > 2)
                {
                    if (Players.Exists(p => p.SeatNumber == Blinds[1]))
                        Blinds[0] = Blinds[1];
                    if (Players.Exists(p => p.SeatNumber == Blinds[2]))
                        Blinds[1] = Blinds[2];

                    var bigBlind = GetNextActiveSeat(Blinds[2]);
                    Blinds[2] = bigBlind;
                }
            }         
        }

        private bool BetweenBlinds(int seatNumber)
        {
            if (Blinds != null)
            {
                var orderedPlayers = Players.OrderBy(p => p.SeatNumber).ToList();
                var dealerIndex = Players.FindIndex(p => p.SeatNumber == Blinds[0]);
                var nextPlayer = dealerIndex;
                int count = 1;
                while (count <= Players.Count)
                {
                    nextPlayer++;
                    if (nextPlayer > Players.Count) nextPlayer = 0;
                    var nextPlayerSeatNumber = orderedPlayers[nextPlayer].SeatNumber;
                    if (nextPlayerSeatNumber == seatNumber)
                        return true;
                    else if (nextPlayerSeatNumber == Blinds[2])
                        return false;
                    count++;
                }
            }

            return false;
        }
    }
}
