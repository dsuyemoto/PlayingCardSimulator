using Dealer;
using System;
using System.Reflection.Metadata.Ecma335;

namespace DealerIntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var _holdem = new TexasHoldem(new Deck(), 10, 20);
            _holdem.SeatPlayer(
                new Player(0, 1000, (o) => { 
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Call }; 
                }), 1);
            _holdem.SeatPlayer(
                new Player(1, 2000, (o) => { 
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Check }; }
                ), 2);
            _holdem.Deal();
            Console.WriteLine("Player0:" + _holdem.Players[0].Cards[0].ToString() + _holdem.Players[0].Cards[1].ToString());
            Console.WriteLine("Player1:" + _holdem.Players[1].Cards[0].ToString() + _holdem.Players[1].Cards[1].ToString());
            Console.Read();

            _holdem.Deal();
            Console.WriteLine("Flop: " + 
                _holdem.Community[0].ToString() + 
                _holdem.Community[1].ToString() +
                _holdem.Community[2].ToString());
            //Console.Read();

            _holdem.Deal();
            Console.WriteLine("Turn: " + _holdem.Community[3].ToString());
            //Console.Read(); 

            _holdem.Deal();
            Console.WriteLine("River: " + _holdem.Community[4].ToString());
            Console.Read();
        }
    }
}
