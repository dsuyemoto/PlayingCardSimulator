using Dealer;
using System;

namespace DealerIntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var _holdem = new TexasHoldem(new Deck());
            _holdem.SeatPlayer(new Player(0), 0);
            _holdem.SeatPlayer(new Player(1), 1);
            _holdem.Deal();
            Console.WriteLine("Player0:" + _holdem.Seats[0].Cards[0].RankValue + _holdem.Seats[0].Cards[0].SuitValue +
                _holdem.Seats[0].Cards[1].RankValue + _holdem.Seats[0].Cards[1].SuitValue
                );
            Console.WriteLine("Player1:" + _holdem.Seats[1].Cards[0].RankValue + _holdem.Seats[1].Cards[0].SuitValue +
                _holdem.Seats[1].Cards[1].RankValue + _holdem.Seats[1].Cards[1].SuitValue);
        }
    }
}
