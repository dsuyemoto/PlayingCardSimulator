using NUnit.Framework;
using System.Linq;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemTests
    {
        TexasHoldem _holdem;

        [SetUp]
        public void Setup()
        {
            _holdem = new TexasHoldem(new Deck(), 10, 20);
            _holdem.SeatPlayer(new Player(0, 1000), 1);
            _holdem.SeatPlayer(new Player(1, 2000), 2);
        }

        [Test()]
        public void DealNoPlayers_CardsDealt_IsFalseTest()
        {
            _holdem.UnseatPlayer(1);
            _holdem.UnseatPlayer(2);
            var dealt = _holdem.Deal();

            Assert.IsFalse(dealt);
        }

        [Test()]
        public void Deal1Player_CardsDealt_IsFalseTest()
        {
            _holdem.UnseatPlayer(2);

            var dealt = _holdem.Deal();

            Assert.IsFalse(dealt);
        }

        [Test]
        public void Deal_Cards_Equals2Test()
        {
            var dealt = _holdem.Deal();

            Assert.AreEqual(2, _holdem.Players[0].Cards.Count);
            Assert.AreEqual(2, _holdem.Players[1].Cards.Count);
            Assert.IsTrue(dealt);
        }

        [Test]
        public void Deal_CommunityCards_AreEqualTest()
        {
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(3, _holdem.Community.Count);
        }

        [Test]
        public void Deal_TurnCards_AreEqualTest()
        {
            _holdem.Deal();
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(4, _holdem.Community.Count);
        }

        [Test]
        public void Deal_RiverCards_AreEqualTest()
        {
            _holdem.Deal();
            _holdem.Deal();
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(5, _holdem.Community.Count);
        }

        [Test]
        public void Deal3Players_Bets_AreEqualTest()
        {
            _holdem.SeatPlayer(new Player(3, 4000), 5);

            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(SMALLBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(200, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(200, _holdem.Players.Single(p => p.SeatNumber == 3).Bet);
        }
    }
}