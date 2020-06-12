using NUnit.Framework;

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
        }

        [Test()]
        public void Deal_NoPlayers_IsFalseTest()
        {
            var dealt = _holdem.Deal();

            Assert.IsFalse(dealt);
        }

        [Test]
        public void Deal_Cards_Equals2Test()
        {
            _holdem.SeatPlayer(new Player(0, 1000), 1);
            _holdem.SeatPlayer(new Player(1, 2000), 2);
            var dealt = _holdem.Deal();

            Assert.AreEqual(2, _holdem.Players[0].Cards.Count);
            Assert.AreEqual(2, _holdem.Players[1].Cards.Count);
            Assert.IsTrue(dealt);
        }

        [Test]
        public void Deal_CommunityCards_AreEqualTest()
        {
            _holdem.SeatPlayer(new Player(0, 1000), 0);
            _holdem.SeatPlayer(new Player(1, 2000), 1);
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(3, _holdem.Community.Count);
        }

        [Test]
        public void Deal_TurnCards_AreEqualTest()
        {
            _holdem.SeatPlayer(new Player(0, 1000), 1);
            _holdem.SeatPlayer(new Player(1, 2000), 2);
            _holdem.Deal();
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(4, _holdem.Community.Count);
        }

        [Test]
        public void Deal_RiverCards_AreEqualTest()
        {
            _holdem.SeatPlayer(new Player(0, 1000), 1);
            _holdem.SeatPlayer(new Player(1, 2000), 2);
            _holdem.Deal();
            _holdem.Deal();
            _holdem.Deal();
            var dealt = _holdem.Deal();

            Assert.IsTrue(dealt);
            Assert.AreEqual(5, _holdem.Community.Count);
        }
    }
}