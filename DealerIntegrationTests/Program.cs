using Dealer;
using System;

namespace DealerIntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player(0) { Chips = 1000 };
            var player2 = new Player(1) { Chips = 2000 };
            var _holdem = new TexasHoldemNoLimit(10, new Deck(), 10, 20);
            _holdem.SeatPlayer(player1, 1);
            _holdem.SeatPlayer(player2, 2);
            _holdem.DealHand();
            Console.WriteLine("Player0:" + _holdem.GetPlayer(player1).Cards[0].ToString() + _holdem.GetPlayer(player1).Cards[1].ToString());
            Console.WriteLine("Player1:" + _holdem.GetPlayer(player2).Cards[0].ToString() + _holdem.GetPlayer(player2).Cards[1].ToString());
            Console.Read();

            _holdem.DealHand();
            Console.WriteLine("Flop: " + 
                _holdem.Community[0].ToString() + 
                _holdem.Community[1].ToString() +
                _holdem.Community[2].ToString());
            //Console.Read();

            _holdem.DealHand();
            Console.WriteLine("Turn: " + _holdem.Community[3].ToString());
            //Console.Read(); 

            _holdem.DealHand();
            Console.WriteLine("River: " + _holdem.Community[4].ToString());
            Console.Read();
        }
    }
}
