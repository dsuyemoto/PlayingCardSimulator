using Dealer;
using NUnit.Framework;
using static Dealer.Card;

namespace DealerTests
{
    public class DeckTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Deck_52cards_AreEqualTest()
        {
            var deck = new Deck();

            var numberOfCards = deck.Cards.Count;

            Assert.AreEqual(52, numberOfCards);
        }

        [Test]
        public void Deck_GetCard_AreEqualTest()
        {
            var deck = new Deck();

            var card = deck.GetCard(0);

            Assert.AreEqual(Rank.Ace, card.RankValue);
            Assert.AreEqual(Suit.Clubs, card.SuitValue);
        }
    }
}