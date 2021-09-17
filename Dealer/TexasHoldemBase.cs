using System.Collections.Generic;
using System.Linq;
using static Dealer.Player;

namespace Dealer
{
    public abstract class TexasHoldemBase : TableBase
    {
        Dictionary<BlindName, Player> _blinds = new Dictionary<BlindName, Player>();

        public abstract decimal SmallBlind { get; set; }
        public abstract decimal BigBlind { get; set; }
        public abstract List<Card> Community { get; set; }
        public abstract bool AutoStartEnabled { get; set; }
        public override Streets Streets { get; set; } = new Streets();

        public override void InitializeStreets()
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
            base.StartBettingRound(startingSeatNumber);
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

        public override void PayWinner()
        {
            var hands = new List<Hand>();
            var players = GetActivePlayers();
            foreach (var player in players)
            {
                var cards = player.Cards;
                foreach (var card in Community)
                    cards.Add(card);
                hands.Add(new Hand(player.Id, cards));
            }
            var bestHand = Deck.BestHand(hands);
            var bestPlayer = GetPlayer(bestHand.PlayerId);
            bestPlayer.Chips += Pot;
            UpdatePlayer(bestPlayer);
            Pot = 0;
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
            if (_blinds.ContainsKey(blindName))
                return _blinds[blindName];

            return null;
        }

        private void SetBlindPlayer(BlindName blindName, Player player)
        {
            if (_blinds.ContainsKey(blindName))
                _blinds[blindName] = player;
            else
                _blinds.Add(blindName, player);
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
            if (GetActivePlayers().Count > 2)
            {
                if (GetBlindPlayer(BlindName.Small) != null)
                    DealerButtonSeatNumber = GetBlindPlayer(BlindName.Small).SeatNumber;
                if (GetBlindPlayer(BlindName.Big) != null)
                    SetBlindPlayer(BlindName.Small, GetBlindPlayer(BlindName.Big));
 
                SetBlindPlayer(BlindName.Big, GetNextActivePlayer(GetBlindPlayer(BlindName.Big).SeatNumber));
            }
            else
            {
                var tempSmallBlindPlayer = GetBlindPlayer(BlindName.Small);
                DealerButtonSeatNumber = GetBlindPlayer(BlindName.Big).SeatNumber;
                SetBlindPlayer(BlindName.Small, GetBlindPlayer(BlindName.Big));
                SetBlindPlayer(BlindName.Big, tempSmallBlindPlayer);
            }
        }

        private bool BetweenBlinds(int seatNumber)
        {
            if (GetBlindPlayer(BlindName.Small) == null ||
                GetBlindPlayer(BlindName.Big) == null)
                return false;

            var currentSeatNumber = GetBlindPlayer(BlindName.Small).SeatNumber;

            do
            {
                currentSeatNumber = GetNextActiveSeat(currentSeatNumber);
                if (currentSeatNumber == seatNumber)
                    return true;
            }
            while (currentSeatNumber != GetBlindPlayer(BlindName.Big).SeatNumber);

            return false;
        }
    }
}
