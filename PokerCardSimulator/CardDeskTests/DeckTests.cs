using CardDeck;
using NUnit.Framework;
using System;
using static CardDeck.Card;

namespace CardDeskTests
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