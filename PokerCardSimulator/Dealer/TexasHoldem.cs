using System.Collections.Generic;
using System.Linq;
using static Dealer.Player;

namespace Dealer
{
    public class TexasHoldem : TableBase
    {

        public TexasHoldem(
            Deck deck,
            double smallBlind,
            double bigBlind,
            int seats = 9,
            int dealerButton = 1,
            bool isTournament = false)
        {
            _deck = deck;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            DealerButton = dealerButton;
            _actionSeatPosition = dealerButton;
            Seats = seats;
            IsTournament = isTournament;
        }

        public void RunStreets()
        {
            SetBlinds();
            int rounds = 0;
            while (rounds < 4)
            {
                Deal();
                GetPlayerAction();
                CollectBets();               
                rounds++;
            }
        }

        
        public bool Deal()
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
            else if (Street == StreetName.Turn || Street == StreetName.River)
            {
                DealCommunityCards(1);
                Street = StreetName.River;

                return true;
            }

            return false;
        }

        protected override Player GetPlayerToAct()
        {
            Player player = null;
            int seatCount = 1;
            while (seatCount < Seats)
            {
                var activePlayers = Players.FindAll(p => p.SitOut == false) as List<Player>;
                if (activePlayers.Exists(p => p.SeatNumber == _actionSeatPosition))
                {
                    player = Players.Single((p) => p.SeatNumber == _actionSeatPosition);
                    player.Options.MinBet = BigBlind;
                    player.Options.AllowedActions = new PlayerAction[]
                    {
                        PlayerAction.Bet,
                        PlayerAction.Check,
                        PlayerAction.Fold
                    };

                    if (_lastBet >= BigBlind)
                    {
                        player.Options.MinBet = _minBet;
                        player.Options.AllowedActions = new PlayerAction[]
                        {
                                PlayerAction.Bet,
                                PlayerAction.Call,
                                PlayerAction.Fold
                        };
                    }
                    IncrementActionSeat();

                    return player;
                }
                IncrementActionSeat();
                seatCount++;
            }

            return player;
        }

        public override bool SeatPlayer(Player player, int seatNumber)
        {
            var result = base.SeatPlayer(player, seatNumber);

            if (Players.Count == 1)
                DealerButton = seatNumber;
            if (Players.Count == 2)
                SetBlinds();

            return result;
        }

        public override bool UnseatPlayer(int seatNumber)
        {
            var result = base.UnseatPlayer(seatNumber);

            if (Players.Count <= 2 && Players.Count > 0)
                while (!Players.Exists(p => p.SeatNumber == DealerButton))
                {
                    DealerButton++;
                    if (DealerButton > Seats)
                        DealerButton = 1;
                }

            return result;
        }
    }
}
