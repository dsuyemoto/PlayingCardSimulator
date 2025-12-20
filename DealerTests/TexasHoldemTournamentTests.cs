using System;
using Dealer;
using NUnit.Framework;
using static Dealer.Player;
using static Dealer.TableBase;

namespace DealerTests
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
        const int DEALERBUTTONSEATNUMBER = 1;

        private void Player1ActionPrompt(object sender, EventArgs eventArgs)
        {

        }

        [SetUp]
        public void Setup()
        {
            _player1 = new Player(1, (s, e) =>
            {
                var table = (TableBase)s;
                var player = table.GetPlayer(_player1);
                player.CurrentAction = PlayerAction.Call;
                player.Bet = BIGBLIND;
                player.Chips = _player1.Chips - SMALLBLIND;
                table.UpdatePlayer(player);
            }) { Chips = 1000 };
            _player2 = new Player(2, (s, e) =>
            {
                var table = (TableBase)s;
                var player = table.GetPlayer(_player2);
                player.CurrentAction = PlayerAction.Check;
                player.Bet = BIGBLIND;
                table.UpdatePlayer(player);
            }) { Chips = 2000 };
            _texasHoldemTournament = new TexasHoldemTournament(new TexasHoldemNoLimit(1, new Deck(), SMALLBLIND, BIGBLIND, 9, DEALERBUTTONSEATNUMBER, 300));
            _texasHoldemTournament.SeatPlayer(_player1, SEAT2);
            _texasHoldemTournament.SeatPlayer(_player2, SEAT3);
            _texasHoldemTournament.SitIn(_player1.SeatNumber);
            _texasHoldemTournament.SitIn(_player2.SeatNumber);
            var streets = new Streets();
            streets.Add(new TexasHoldemPlayerStreet(_texasHoldemTournament, 2, true, StreetName.PreFlop));
            _texasHoldemTournament.Streets = streets;
        }

        [Test()]
        public void DealCards_Cards_AreEqualTest()
        {            
            _texasHoldemTournament.Streets.DealCards();

            Assert.AreEqual(2, _texasHoldemTournament.GetPlayer(SEAT2).Cards.Count);
            Assert.AreEqual(2, _texasHoldemTournament.GetPlayer(SEAT3).Cards.Count);
            Assert.AreEqual(SMALLBLIND, _texasHoldemTournament.GetPlayer(SEAT2).Bet);
            Assert.AreEqual(BIGBLIND, _texasHoldemTournament.GetPlayer(SEAT3).Bet);
        }
    }
}