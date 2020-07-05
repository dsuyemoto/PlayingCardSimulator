using System.Collections.Generic;
using static Dealer.TableBase;

namespace Dealer
{
    public class TexasHoldemView
    {
        public int DealerButton { get; protected set; }
        public List<Player> Players { get; protected set; } = new List<Player>();
        public int Seats { get; protected set; }
        public List<Card> Community { get; protected set; } = new List<Card>();
        public double Pot { get; protected set; }
        public double SmallBlind { get; protected set; }
        public double BigBlind { get; protected set; }
        public StreetName Street { get; protected set; } = StreetName.PreFlop;

        public TexasHoldemView(TexasHoldemBase texasHoldemBase, int playerId)
        {
            DealerButton = texasHoldemBase.DealerButton;
            foreach (var otherPlayer in texasHoldemBase.Players)
            {
                if (playerId == otherPlayer.Id)
                {
                    Players.Add(otherPlayer);
                }
                else
                {
                    var seatedPlayer = new Player(otherPlayer.Id, otherPlayer.Chips);
                    seatedPlayer.Bet = otherPlayer.Bet;
                    seatedPlayer.SeatNumber = otherPlayer.SeatNumber;
                    seatedPlayer.SitOut = otherPlayer.SitOut;
                    Players.Add(seatedPlayer);
                }
            }

            Seats = texasHoldemBase.Seats;
            Community = texasHoldemBase.Community;
            Pot = texasHoldemBase.Pot;
            Street = texasHoldemBase.Street;
        }
    }
}
