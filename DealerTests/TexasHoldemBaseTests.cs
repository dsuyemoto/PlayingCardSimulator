using Moq;
using NuGet.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Dealer.Player;
using static Dealer.TableBase;

namespace Dealer.Tests
{
    [TestFixture()]
    public class TexasHoldemBaseTests
    {
        TexasHoldemBase _holdem;
        Player _player1;
        Player _player2;
        Player _player3;
        EventHandler _player1EventHandler;
        EventHandler _player2EventHandler;
        EventHandler _player3EventHandler;

        const decimal SMALLBLIND = 100;
        const decimal BIGBLIND = 200;
        const int DEALERBUTTON = 3;
        const decimal PLAYERCHIPS1 = 1000;
        const decimal PLAYERCHIPS2 = 2000;
        const decimal PLAYERCHIPS3 = 3000;
        const int PLAYERID1 = 1;
        const int PLAYERID2 = 2;
        const int PLAYERID3 = 3;
        const int SEAT1 = 1;
        const int SEAT2 = 2;
        const int SEAT3 = 3;
        const int TABLEID = 100;

        public class TableClassProvider
        {
            public static IEnumerable<TexasHoldemBase> TableBaseClasses()
            {
                yield return new TexasHoldemCash(new TexasHoldemNoLimit(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON, 300));
                yield return new TexasHoldemTournament(new TexasHoldemNoLimit(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON, 300));
            }
        }

        public TexasHoldemBaseTests()
        {
            _player1EventHandler = (s, e) =>
            {
                var table = (TableBase)s;
                var player = table.GetPlayer(_player1);
                player.CurrentAction = PlayerAction.Call;
                player.Bet = BIGBLIND;
                player.Chips = _player1.Chips - SMALLBLIND;
                table.UpdatePlayer(player);
            };
            _player2EventHandler = (s, e) =>
            {
                var table = (TableBase)s;
                var player = table.GetPlayer(_player2);
                player.CurrentAction = PlayerAction.Check;
                player.Bet = BIGBLIND;
                table.UpdatePlayer(player);
            };
            _player3EventHandler = (s, e) =>
            {
                var table = (TableBase)s;
                var player = table.GetPlayer(_player3);
                player.CurrentAction = PlayerAction.Call;
                player.Bet = BIGBLIND;
                player.Chips = _player3.Chips - BIGBLIND;
                table.UpdatePlayer(player);
            };
        }

        private void HoldemSetup()
        {
            var streets = new Streets();
            streets.Add(new TexasHoldemPlayerStreet(_holdem, 2, true, StreetName.PreFlop));
            _holdem.Streets = streets;
            _holdem.SeatPlayer(_player1, SEAT1);
            _holdem.SitOut(_player1.SeatNumber);
            _holdem.SeatPlayer(_player2, SEAT2);
            _holdem.SitOut(_player2.SeatNumber);
        }

        [SetUp]        
        public void Setup()
        {
            _player1 = new Player(PLAYERID1, _player1EventHandler) { Chips = PLAYERCHIPS1 };
            _player2 = new Player(PLAYERID2, _player2EventHandler) { Chips = PLAYERCHIPS2 };
            _player3 = new Player(PLAYERID3, _player3EventHandler) { Chips = PLAYERCHIPS3 };
            _holdem = new TexasHoldemCash(new TexasHoldemNoLimit(TABLEID, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTON, 300));
            _holdem.AutoStartEnabled = false;
            HoldemSetup();
        }

        [Test, TestCaseSource(typeof(TableClassProvider), "TableBaseClasses")]
        public void Unseat_NoPlayers_IsFalseTest(TexasHoldemBase texasHoldemBase)
        {
            _holdem = texasHoldemBase;
            HoldemSetup();
            _holdem.UnseatPlayer(SEAT1);
            _holdem.UnseatPlayer(SEAT2);
            _holdem.StartGame();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void StartGame_1Player_RunningIsFalseTest()
        {
            _holdem.UnseatPlayer(SEAT1);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.StartGame();

            Assert.IsFalse(_holdem.IsGameRunning);
        }

        [Test]
        public void DealCards_Cards_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.Streets.Add(new TexasHoldemCommunityStreet(_holdem, 3, false, StreetName.Flop));
            _holdem.Streets.Add(new TexasHoldemCommunityStreet(_holdem, 1, false, StreetName.Turn));
            _holdem.Streets.Add(new TexasHoldemCommunityStreet(_holdem, 1, false, StreetName.River));

            _holdem.Streets.DealCards();
            Assert.AreEqual(2, _holdem.GetPlayer(_player1.SeatNumber).Cards.Count);
            Assert.AreEqual(2, _holdem.GetPlayer(_player2.SeatNumber).Cards.Count);
            _holdem.Streets.Next();
            _holdem.Streets.DealCards();
            Assert.AreEqual(3, _holdem.Community.Count);
            _holdem.Streets.Next();
            _holdem.Streets.DealCards();
            Assert.AreEqual(4, _holdem.Community.Count);
            _holdem.Streets.Next();
            _holdem.Streets.DealCards();
            Assert.AreEqual(5, _holdem.Community.Count);
        }

        [Test]
        public void DealHand_2Players_AreEqualTest()
        {
            _holdem.Streets.Add(new TexasHoldemPlayerStreet(_holdem, 1, false, StreetName.Flop));
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);            

            _holdem.DealHand();
            
            Assert.AreEqual(PlayerAction.Call, _holdem.GetPlayer(_player1.SeatNumber).CurrentAction);
            Assert.AreEqual(PlayerAction.Check, _holdem.GetPlayer(_player2.SeatNumber).CurrentAction);
            Assert.AreEqual(0, _holdem.GetPlayer(_player1.SeatNumber).Bet);
            Assert.AreEqual(0, _holdem.GetPlayer(_player2.SeatNumber).Bet);
            Assert.AreEqual(0, _holdem.Pot);
        }

        [Test]
        public void DealCards_3Players_AreEqualTest()
        {
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);
            _holdem.Streets.DealCards();
            _holdem.Streets.StartBettingRound(_holdem.DealerButtonSeatNumber);
            _holdem.CollectBets();
            _holdem.Streets.Next();

        }

        [Test]
        public void DealHand_3Players_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);

            _holdem.DealHand();

            Assert.AreEqual(0, _holdem.GetPlayer(SEAT1).Bet);
            Assert.AreEqual(0, _holdem.GetPlayer(SEAT2).Bet);
            Assert.AreEqual(0, _holdem.GetPlayer(SEAT3).Bet);
            Assert.AreEqual(PlayerAction.Call, _holdem.GetPlayer(SEAT1).CurrentAction);
            Assert.AreEqual(PlayerAction.Check, _holdem.GetPlayer(SEAT2).CurrentAction);
            Assert.AreEqual(PlayerAction.Call, _holdem.GetPlayer(SEAT3).CurrentAction);
            Assert.AreEqual(0, _holdem.Pot);
        }

        [Test]
        public void DealHand_3PlayerTimeout_SitoutTrueTest()
        {
            _holdem.PlayerTimeoutMilliseconds = 10;
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);

            _holdem.DealHand();

            Assert.AreEqual(PlayerAction.Fold, _holdem.GetPlayer(_player3).CurrentAction);
            Assert.IsTrue(_holdem.GetPlayer(_player3).SittingOut);
            Assert.AreEqual(PlayerAction.Call, _holdem.GetPlayer(SEAT1).CurrentAction);
            Assert.AreEqual(PlayerAction.Check, _holdem.GetPlayer(SEAT2).CurrentAction);
            Assert.AreEqual(0, _holdem.Pot);
        }

        [Test]
        public void DealHand_3PlayerFold_WinnerIsTrueTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);

            _holdem.DealHand();
        }

        [Test]
        public void DealHand_1PlayerTimeout_SitoutTrueTest()
        {
            _holdem.PlayerTimeoutMilliseconds = 1;
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);

            _holdem.DealHand();

            Assert.AreEqual(PlayerAction.Fold, _holdem.GetPlayer(_player3).CurrentAction);
            Assert.AreEqual(PlayerAction.Check, _holdem.GetPlayer(_player2).CurrentAction);
            Assert.AreEqual(PlayerAction.Call, _holdem.GetPlayer(_player1).CurrentAction);
            Assert.IsTrue(_holdem.GetPlayer(_player3).SittingOut);
            Assert.IsFalse(_holdem.GetPlayer(_player2).SittingOut);
            Assert.IsFalse(_holdem.GetPlayer(_player1).SittingOut);
        }

        [Test]
        public void DealHand_CollectBets_AreEqualTest()
        {
            _holdem.SitIn(_player1.SeatNumber);
            _holdem.SitIn(_player2.SeatNumber);
            _holdem.SeatPlayer(_player3, 3);
            _holdem.SitIn(_player3.SeatNumber);

            _holdem.DealHand();

            Assert.AreEqual(0, _holdem.Pot);
        }

        //[Test]
        //public void GetTableView_TableProperties_AreEqualTest()
        //{
        //    _holdem.SitIn(_player1.SeatNumber);
        //    _holdem.SitIn(_player2.SeatNumber);
        //    _holdem.Streets.DealCards();

        //    var tableView = _holdem.GetTableView(PLAYERID1);

        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Chips, tableView.Players.Single(p => p.Id == PLAYERID1).Chips);
        //    Assert.IsNotEmpty(tableView.Players.Single(p => p.Id == PLAYERID1).Cards);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Bet, tableView.Players.Single(p => p.Id == PLAYERID1).Bet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID1).SeatNumber);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).SitOut, tableView.Players.Single(p => p.Id == PLAYERID1).SitOut);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.MinBet, tableView.Players.Single(p => p.Id == PLAYERID1).Options.MinBet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions, tableView.Players.Single(p => p.Id == PLAYERID1).Options.AllowedActions);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Chips, tableView.Players.Single(p => p.Id == PLAYERID2).Chips);
        //    Assert.IsEmpty(tableView.Players.Single(p => p.Id == PLAYERID2).Cards);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).Bet, tableView.Players.Single(p => p.Id == PLAYERID2).Bet);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SeatNumber, tableView.Players.Single(p => p.Id == PLAYERID2).SeatNumber);
        //    Assert.AreEqual(_holdem.Players.Single(p => p.Id == PLAYERID2).SitOut, tableView.Players.Single(p => p.Id == PLAYERID2).SitOut);
        //}
    }
}