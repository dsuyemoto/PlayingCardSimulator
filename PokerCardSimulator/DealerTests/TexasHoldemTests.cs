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

        const double SMALLBLIND = 100;
        const double BIGBLIND = 200;
        const int DEALERBUTTON = 3;
        const double PLAYERCHIPS1 = 1000;
        const double PLAYERCHIPS2 = 2000;
        const int PLAYERID1 = 1;
        const int PLAYERID2 = 2;
        const int TABLEID = 100;

        public TexasHoldemTests()
        {
            _player1 = new Player(PLAYERID1, PLAYERCHIPS1);
            _player2 = new Player(PLAYERID2, PLAYERCHIPS2);
        }

        [SetUp]
        public void Setup()
        {
            _holdem = new TexasHoldemNoLimit(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON);
            _holdem.SeatPlayer(_player1, 1);
            _holdem.SitOut(1);
            _holdem.SeatPlayer(_player2, 2);
        }

        [Test()]
        public void Unseat_NoPlayers_IsFalseTest()
        {
            _holdem.UnseatPlayer(1);
            _holdem.UnseatPlayer(2);
            _holdem.StartGame();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void Deal_1Player_IsFalseTest()
        {
            _holdem.UnseatPlayer(1);
            _holdem.StartGame();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void Deal_Cards_AreEqualTest()
        {
            _holdem.SitIn(1);
            _holdem.Players[0].CurrentAction = PlayerAction.Call;
            _holdem.Players[0].Bet = BIGBLIND;
            _holdem.Players[1].CurrentAction = PlayerAction.Check;
            _holdem.Players[1].Bet = BIGBLIND;
            _holdem.DealHand();

            Assert.AreEqual(2, _holdem.Players[0].Cards.Count);
            Assert.AreEqual(2, _holdem.Players[1].Cards.Count);
            Assert.AreEqual(PLAYERCHIPS1 - 200, _holdem.Players[0].Chips);
            Assert.AreEqual(PLAYERCHIPS2 - 200, _holdem.Players[1].Chips);
        }

        [Test]
        public void Deal_CommunityCards_AreEqualTest()
        {
            _holdem.SitIn(1);
            _holdem.Players[0].CurrentAction = PlayerAction.Call;
            _holdem.Players[0].Bet = BIGBLIND;
            _holdem.Players[1].CurrentAction = PlayerAction.Check;
            _holdem.Players[1].Bet = BIGBLIND;
            _holdem.DealHand();
            _holdem.Players[0].CurrentAction = PlayerAction.Call;
            _holdem.Players[0].Bet = BIGBLIND;
            _holdem.Players[1].CurrentAction = PlayerAction.Check;
            _holdem.Players[1].Bet = BIGBLIND;
            _holdem.DealHand();

            Assert.AreEqual(3, _holdem.Community.Count);
        }

        //[Test]
        //public void Deal_TurnCards_AreEqualTest()
        //{
        //    _holdem.Deal();
        //    _holdem.Deal();
        //    var dealt = _holdem.Deal();

        //    Assert.IsTrue(dealt);
        //    Assert.AreEqual(4, _holdem.Community.Count);
        //}

        //[Test]
        //public void Deal_RiverCards_AreEqualTest()
        //{
        //    _holdem.Deal();
        //    _holdem.Deal();
        //    _holdem.Deal();
        //    var dealt = _holdem.Deal();

        //    Assert.IsTrue(dealt);
        //    Assert.AreEqual(5, _holdem.Community.Count);
        //}

        //[Test]
        //public void Action_2Players_Blinds_AreEqualTest()
        //{
        //    _holdem = new TexasHoldem(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON);
        //    _holdem.SeatPlayer(new Player(PLAYERID1, PLAYERCHIPS1), 1);
        //    _holdem.SeatPlayer(new Player(PLAYERID2, PLAYERCHIPS2), 2);

        //    _holdem.StartPlayerAction();

        //    Assert.AreEqual(SMALLBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        //    Assert.AreEqual(Player.PlayerAction.Fold, _holdem.Players.Single(p => p.SeatNumber == 1).LastAction);
        //    Assert.AreEqual(Player.PlayerAction.Check, _holdem.Players.Single(p => p.SeatNumber == 2).LastAction);
        //}

        //[Test]
        //public void Action_2Players_Blinds_AreEqual2Test()
        //{
        //    _holdem.Players[0].LastAction = PlayerAction.Call;
        //    _holdem.Players[1].LastAction = PlayerAction.Check;
        //    _holdem.StartPlayerAction();

        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        //}

        //[Test]
        //public void Action_3Players_Blinds_AreEqualTest()
        //{
        //    _holdem.SeatPlayer(new Player(PLAYERID1, PLAYERCHIPS1), 1);
        //    _holdem.SeatPlayer(new Player(PLAYERID2, PLAYERCHIPS2), 2);

        //    _holdem.StartPlayerAction();

        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        //}

        //[Test]
        //public void Action_Bets_AreEqualTest()
        //{
        //    _holdem = new TexasHoldem(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON);
        //    _holdem.SeatPlayer(new Player(PLAYERID1, PLAYERCHIPS1), 1);
        //    _holdem.SeatPlayer(new Player(PLAYERID2, PLAYERCHIPS2), 2);

        //    _holdem.StartPlayerAction();

        //    Assert.AreEqual(BIGBLIND, _holdem.Players.Single(p => p.SeatNumber == 1).Bet);
        //    Assert.AreEqual(BIGBLIND * 3, _holdem.Players.Single(p => p.SeatNumber == 2).Bet);
        //}

        //[Test]
        //public void GetTableView_TableProperties_AreEqualTest()
        //{
        //    _holdem.Deal();
        //    var tableView = _holdem.GetTableView(PLAYERID1);

        //    Assert.AreEqual(_holdem.DealerButton, tableView.DealerButton);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Chips, tableView.Players.Single(p => p.Id == PLAYERID1).Chips);
        //    Assert.IsNotEmpty(tableView.Players.Single(p => p.Id == PLAYERID1).Cards);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Bet, tableView.Players.Single(p => p.Id == PLAYERID1).Bet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).LastAction, tableView.Players.Single(p => p.Id == PLAYERID1).LastAction);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID1).SeatNumber);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SitOut, tableView.Players.Single(p => p.Id == PLAYERID1).SitOut);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.MinBet, tableView.Players.Single(p => p.Id == PLAYERID1).Options.MinBet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions, tableView.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Prompt, tableView.Players.Single(p => p.Id == PLAYERID1).Prompt);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Chips, tableView.Players.Single(p => p.Id == PLAYERID2).Chips);
        //    Assert.IsEmpty(tableView.Players.Single(p => p.Id == PLAYERID2).Cards);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Bet, tableView.Players.Single(p => p.Id == PLAYERID2).Bet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID2).SeatNumber);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SitOut, tableView.Players.Single(p => p.Id == PLAYERID2).SitOut);
        //}
    }
}