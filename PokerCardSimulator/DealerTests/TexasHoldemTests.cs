using NUnit.Framework;
using System.Linq;
using System.Runtime.CompilerServices;
using static Dealer.Player;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemTests
    {
        TexasHoldemBase _holdem;
        Player _player1;
        Player _player2;
        Player _player3;

        const double SMALLBLIND = 100;
        const double BIGBLIND = 200;
        const int DEALERBUTTON = 3;
        const double PLAYERCHIPS1 = 1000;
        const double PLAYERCHIPS2 = 2000;
        const double PLAYERCHIPS3 = 3000;
        const int PLAYERID1 = 1;
        const int PLAYERID2 = 2;
        const int PLAYERID3 = 3;
        const int SEAT1 = 1;
        const int SEAT2 = 2;
        const int TABLEID = 100;

        public TexasHoldemTests()
        {
            _player1 = new Player(PLAYERID1, PLAYERCHIPS1);
            _player2 = new Player(PLAYERID2, PLAYERCHIPS2);
            _player3 = new Player(PLAYERID3, PLAYERCHIPS3);
        }

        [SetUp]
        public void Setup()
        {
            _holdem = new TexasHoldemNoLimit(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON);
            _holdem.SeatPlayer(_player1, SEAT1);
            _holdem.SitOut(_player1.SeatNumber);
            _holdem.SeatPlayer(_player2, SEAT2);
            _holdem.SitOut(_player2.SeatNumber);
        }

        [Test()]
        public void Unseat_NoPlayers_IsFalseTest()
        {
            _holdem.UnseatPlayer(SEAT1);
            _holdem.UnseatPlayer(SEAT2);
            _holdem.StartGame();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void Deal_1Player_IsFalseTest()
        {
            _holdem.UnseatPlayer(SEAT1);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.StartGame();
            _holdem.RunningGame.Wait();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void Deal_Cards_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.DealStreet();

            Assert.AreEqual(2, _holdem.Players[0].Cards.Count);
            Assert.AreEqual(2, _holdem.Players[1].Cards.Count);
        }

        [Test]
        public void Deal_CommunityCards_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.DealStreet();
            _holdem.DealStreet();

            Assert.AreEqual(3, _holdem.Community.Count);
        }

        [Test]
        public void Deal_TurnCards_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.DealStreet();
            _holdem.DealStreet();
            var dealt = _holdem.DealStreet();

            Assert.IsTrue(dealt);
            Assert.AreEqual(4, _holdem.Community.Count);
        }

        [Test]
        public void Deal_RiverCards_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.DealStreet();
            _holdem.DealStreet();
            _holdem.DealStreet();
            var dealt = _holdem.DealStreet();

            Assert.IsTrue(dealt);
            Assert.AreEqual(5, _holdem.Community.Count);
        }

        [Test]
        public void StartPlayerAction_2Players_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.Players[0].CurrentAction = PlayerAction.Call;
            _holdem.Players[0].Bet = BIGBLIND;
            _holdem.Players[1].CurrentAction = PlayerAction.Check;
            _holdem.Players[1].Bet = BIGBLIND;

            _holdem.StartPlayerAction();

            Assert.AreEqual(PLAYERCHIPS1 - 200, _holdem.Players[0].Chips);
            Assert.AreEqual(PLAYERCHIPS2 - 200, _holdem.Players[1].Chips);
            Assert.AreEqual(PlayerAction.Call, _holdem.Players[0].CurrentAction);
            Assert.AreEqual(PlayerAction.Check, _holdem.Players[1].CurrentAction);
            Assert.AreEqual(200, _holdem.Players[0].Bet);
            Assert.AreEqual(200, _holdem.Players[1].Bet);
        }

        [Test]
        public void StartPlayerAction_3Players_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);
            _holdem.Players[0].CurrentAction = PlayerAction.Call;
            _holdem.Players[0].Bet = BIGBLIND;
            _holdem.Players[1].CurrentAction = PlayerAction.Check;
            _holdem.Players[1].Bet = BIGBLIND;
            _holdem.Players[2].CurrentAction = PlayerAction.Call;
            _holdem.Players[2].Bet = BIGBLIND;

            _holdem.StartPlayerAction();

            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 3).Bet);
            Assert.AreEqual(PlayerAction.Call, _holdem.Players.Single(p => p.SeatNumber == 1).CurrentAction);
            Assert.AreEqual(PLAYERCHIPS1 - 200, _holdem.Players.Single(p => p.SeatNumber == 1).Chips);
            Assert.AreEqual(PlayerAction.Check, _holdem.Players.Single(p => p.SeatNumber == 2).CurrentAction);
            Assert.AreEqual(PLAYERCHIPS2 - 200, _holdem.Players.Single(p => p.SeatNumber == 2).Chips);
            Assert.AreEqual(PlayerAction.Call, _holdem.Players.Single(p => p.SeatNumber == 3).CurrentAction);
            Assert.AreEqual(PLAYERCHIPS3 - 200, _holdem.Players.Single(p => p.SeatNumber == 3).Chips);
        }

        [Test]
        public void GetTableView_TableProperties_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.DealStreet();

            var tableView = _holdem.GetTableView(PLAYERID1);

            Assert.AreEqual(_holdem.DealerButtonSeatNumber, tableView.DealerButton);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Chips, tableView.Players.Single(p => p.Id == PLAYERID1).Chips);
            Assert.IsNotEmpty(tableView.Players.Single(p => p.Id == PLAYERID1).Cards);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Bet, tableView.Players.Single(p => p.Id == PLAYERID1).Bet);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID1).SeatNumber);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SitOut, tableView.Players.Single(p => p.Id == PLAYERID1).SitOut);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.MinBet, tableView.Players.Single(p => p.Id == PLAYERID1).Options.MinBet);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions, tableView.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Chips, tableView.Players.Single(p => p.Id == PLAYERID2).Chips);
            Assert.IsEmpty(tableView.Players.Single(p => p.Id == PLAYERID2).Cards);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Bet, tableView.Players.Single(p => p.Id == PLAYERID2).Bet);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID2).SeatNumber);
            Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SitOut, tableView.Players.Single(p => p.Id == PLAYERID2).SitOut);
        }
    }
}