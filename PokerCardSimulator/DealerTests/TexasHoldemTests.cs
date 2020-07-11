using NUnit.Framework;
using System.Linq;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemTests
    {
        TexasHoldem _holdem;
        const double SMALLBLIND = 100;
        const double BIGBLIND = 200;

        [SetUp]
        public void Setup()
        {
            _holdem = new TexasHoldem(new Deck(), SMALLBLIND, BIGBLIND, 9, 3);
            _holdem.SeatPlayer(new Player(0, 1000, (o) => {
                var allowedActions = o.AllowedActions.ToList();
                if (allowedActions.Contains(Player.PlayerAction.Call))
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Call };
                else
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Check };
            }), 1);
            _holdem.SeatPlayer(new Player(1, 2000, (o) => {
                var allowedActions = o.AllowedActions.ToList();
                if (allowedActions.Contains(Player.PlayerAction.Call))
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Call };
                else
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Check };
            }), 2);
        }

        [Test()]
        public void Deal_NoPlayers_IsFalseTest()
        {
            _holdem.UnseatPlayer(1);
            _holdem.UnseatPlayer(2);
            var dealt = _holdem.Deal();

            Assert.IsFalse(dealt);
        }

        [Test]
        public void Deal_1Player_IsFalseTest()
        {
            _holdem.UnseatPlayer(1);
            var dealt = _holdem.Deal();

            Assert.IsFalse(dealt);
        }

        [Test]
        public void Deal_ButtonMove_AreEqualTest()
        {
            _holdem.UnseatPlayer(1);
            var dealt = _holdem.Deal();

            Assert.AreEqual(2, _holdem.DealerButton);
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
        public void Action_2Players_Blinds_AreEqualTest()
        {
            _holdem = new TexasHoldem(new Deck(), SMALLBLIND, BIGBLIND, 9, 3);
            _holdem.SeatPlayer(new Player(0, 1000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Fold };
            }), 1);
            _holdem.SeatPlayer(new Player(1, 2000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Check };
            }), 2);

            _holdem.GetPlayerAction();

            Assert.AreEqual(SMALLBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(Player.PlayerAction.Fold, _holdem.Players.Single(p => p.SeatNumber == 1).Action);
            Assert.AreEqual(Player.PlayerAction.Check, _holdem.Players.Single(p => p.SeatNumber == 2).Action);
        }

        [Test]
        public void Action_2Players_Blinds_AreEqual2Test()
        {
            _holdem.GetPlayerAction();

            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        }

        [Test]
        public void Action_3Players_Blinds_AreEqualTest()
        {
            _holdem = new TexasHoldem(new Deck(), SMALLBLIND, BIGBLIND, 9, 3);
            _holdem.SeatPlayer(new Player(0, 1000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Call };
            }), 1);
            _holdem.SeatPlayer(new Player(1, 2000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Check };
            }), 2);
            _holdem.SeatPlayer(new Player(2, 2000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Call };
            }), 5);

            _holdem.GetPlayerAction();

            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 5).Bet);
        }

        [Test]
        public void Action_Bets_AreEqualTest()
        {
            _holdem = new TexasHoldem(new Deck(), SMALLBLIND, BIGBLIND, 9, 3);
            _holdem.SeatPlayer(new Player(0, 1000, (o) => {
                var allowedActions = o.AllowedActions.ToList();
                if (allowedActions.Contains(Player.PlayerAction.Call))
                    return new PromptActions() { PlayerAction = Player.PlayerAction.Call };
                return new PromptActions() { PlayerAction = Player.PlayerAction.Fold };
            }), 1);
            _holdem.SeatPlayer(new Player(1, 2000, (o) => {
                return new PromptActions() { PlayerAction = Player.PlayerAction.Bet, Bet = BIGBLIND * 3 };
            }), 2);

            _holdem.GetPlayerAction();

            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(BIGBLIND * 3, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        }
    }
}