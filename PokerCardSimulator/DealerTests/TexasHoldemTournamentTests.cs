using NUnit.Framework;
using System.Linq;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemTournamentTests
    {
        Player _player1;
        Player _player2;
        TexasHoldemBase _texasHoldemTournament;

        const double BIGBLIND = 200;
        const double SMALLBLIND = 100;

        [SetUp]
        public void Setup()
        {
            _player1 = new Player(1, 1000);
            _player2 = new Player(2, 2000);
            _texasHoldemTournament = new TexasHoldemTournament(new TexasHoldemNoLimit(1, new Deck(), SMALLBLIND, BIGBLIND));
            _texasHoldemTournament.SeatPlayer(_player1, 2);
            _texasHoldemTournament.SeatPlayer(_player2, 3);
            _texasHoldemTournament.SitIn(_player1.SeatNumber);
            _texasHoldemTournament.SitIn(_player2.SeatNumber);
        }

        [Test()]
        public void Deal_Cards_AreEqualTest()
        {
            _texasHoldemTournament.DealHand();

            Assert.AreEqual(2, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 2).Cards.Count);
            Assert.AreEqual(2, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 3).Cards.Count);
        }

        [Test]
        public void Deal_Blinds_AreEqualTest()
        {
            _texasHoldemTournament.DealHand();

            Assert.AreEqual(SMALLBLIND, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(BIGBLIND, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 3).Bet);
        }
    }
}