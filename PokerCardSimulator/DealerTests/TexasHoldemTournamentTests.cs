using NUnit.Framework;
using System.Linq;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemTournamentTests
    {
        [Test()]
        public void TexasHoldemTournamentTest()
        {
            var player1 = new Player(1, 1000);
            var player2 = new Player(2, 2000);
            var texasHoldemTournament = new TexasHoldemTournament(new TexasHoldemNoLimit(1, new Deck(), 100, 200));
            texasHoldemTournament.SeatPlayer(player1, 2);
            texasHoldemTournament.SeatPlayer(player2, 3);
            texasHoldemTournament.SitIn(player1.SeatNumber);
            texasHoldemTournament.SitIn(player2.SeatNumber);

            texasHoldemTournament.DealHand();

            Assert.AreEqual(2, texasHoldemTournament.Players.Single(p => p.SeatNumber == 2).Cards.Count);
            Assert.AreEqual(2, texasHoldemTournament.Players.Single(p => p.SeatNumber == 3).Cards.Count);
        }
    }
}