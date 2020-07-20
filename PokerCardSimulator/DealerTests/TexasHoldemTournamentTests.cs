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

        const decimal BIGBLIND = 200;
        const decimal SMALLBLIND = 100;
        const int SEAT2 = 2;
        const int SEAT3 = 3;

        [SetUp]
        public void Setup()
        {
            _player1 = new Player(1) { Chips = 1000 };
            _player2 = new Player(2) { Chips = 2000 };
            _texasHoldemTournament = new TexasHoldemTournament(new TexasHoldemNoLimit(1, new Deck(), SMALLBLIND, BIGBLIND));
            _texasHoldemTournament.SeatPlayer(_player1, SEAT2);
            _texasHoldemTournament.SeatPlayer(_player2, SEAT3);
            _texasHoldemTournament.SitIn(_player1.SeatNumber);
            _texasHoldemTournament.SitIn(_player2.SeatNumber);
        }

        [Test()]
        public void Deal_Cards_AreEqualTest()
        {
            _player1.CurrentAction = Player.PlayerAction.Call;
            _player2.CurrentAction = Player.PlayerAction.Check;
            _texasHoldemTournament.UpdatePlayer(_player1);
            _texasHoldemTournament.UpdatePlayer(_player2);

            _texasHoldemTournament.DealStreet();

            Assert.AreEqual(2, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 2).Cards.Count);
            Assert.AreEqual(2, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 3).Cards.Count);
        }

        [Test]
        public void Deal_Blinds_AreEqualTest()
        {
            _player1.CurrentAction = Player.PlayerAction.Call;
            _player2.CurrentAction = Player.PlayerAction.Check;
            _player1.Bet = SMALLBLIND;
            _player2.Bet = BIGBLIND;
            _texasHoldemTournament.UpdatePlayer(_player1);
            _texasHoldemTournament.UpdatePlayer(_player2);

            _texasHoldemTournament.StartBettingRound();

            Assert.AreEqual(SMALLBLIND, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 2).Bet);
            Assert.AreEqual(BIGBLIND, _texasHoldemTournament.Players.Single(p => p.SeatNumber == 3).Bet);
        }
    }
}