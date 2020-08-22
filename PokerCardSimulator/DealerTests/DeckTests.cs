using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using static Dealer.Card;
using static Dealer.Deck;

namespace Dealer.Tests
{
    [TestFixture()]
    public class DeckTests
    {
        Deck _deck;

        [SetUp]
        public void Setup()
        {
            _deck = new Deck();
        }

        [Test]
        public void Deck_52cards_AreEqualTest()
        {
            var numberOfCards = _deck.Cards.Count;

            Assert.AreEqual(52, numberOfCards);
        }

        [Test]
        public void Cards_Card_AreEqualTest()
        {
            Assert.AreEqual(Rank.Ace, _deck.Cards[0].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[0].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[1].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[1].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[2].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[2].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[3].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[3].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[4].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[4].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[5].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[5].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[6].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[6].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[7].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[7].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[8].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[8].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[9].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[9].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[10].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[10].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[11].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[11].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[12].RankValue);
            Assert.AreEqual(Suit.Clubs, _deck.Cards[12].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[13].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[13].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[14].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[14].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[15].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[15].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[16].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[16].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[17].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[17].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[18].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[18].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[19].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[19].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[20].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[20].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[21].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[21].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[22].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[22].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[23].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[23].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[24].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[24].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[25].RankValue);
            Assert.AreEqual(Suit.Hearts, _deck.Cards[25].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[26].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[26].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[27].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[27].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[28].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[28].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[29].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[29].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[30].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[30].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[31].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[31].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[32].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[32].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[33].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[33].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[34].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[34].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[35].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[35].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[36].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[36].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[37].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[37].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[38].RankValue);
            Assert.AreEqual(Suit.Diamonds, _deck.Cards[38].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[39].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[39].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[40].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[40].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[41].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[41].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[42].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[42].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[43].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[43].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[44].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[44].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[45].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[45].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[46].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[46].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[47].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[47].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[48].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[48].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[49].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[49].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[50].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[50].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[51].RankValue);
            Assert.AreEqual(Suit.Spades, _deck.Cards[51].SuitValue);
        }

        [Test]
        public void GetCard_Card_AreEqualTest()
        {
            var card = _deck.GetCard(0);

            Assert.AreEqual(Rank.Ace, card.RankValue);
            Assert.AreEqual(Suit.Clubs, card.SuitValue);
            Assert.IsNull(_deck.Cards[0]);
        }

        [Test]
        public void ContainsCard_Card_AreEqualTest()
        {
            var expectedCard = new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs };

            Assert.IsTrue(Deck.ContainsCards(_deck.Cards, expectedCard));
        }

        [Test]
        public void DeckExcludedCards_Card_AreEqualTest()
        {
            var cards = new List<Card>();
            var excludedCard = new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs };
            cards.Add(excludedCard);

            _deck = new Deck(cards);

            Assert.IsFalse(Deck.ContainsCards(_deck.Cards, excludedCard));
        }

        [Test]
        public void RankHands_Cards_InOrderTest()
        {
            var hand1 = new Card[] {
                new Card() {  RankValue = Rank.Ace, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.King, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Queen, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Ten, SuitValue = Suit.Clubs }
            };
            var hand2 = new Card[] {
                new Card() {  RankValue = Rank.Ace, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Hearts },
                new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Jack, SuitValue = Suit.Diamonds }
            };
            var hand3 = new Card[] {
                new Card() {  RankValue = Rank.Ace, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Two, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Three, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Four, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Five, SuitValue = Suit.Spades }
            };
            
            var hands = new List<Hand>();
            hands.Add(new Hand(2, hand2.ToList()));
            hands.Add(new Hand(1, hand1.ToList()));
            hands.Add(new Hand(3, hand3.ToList()));

            var bestHand = Deck.BestHand(hands);

            Assert.AreEqual(hand1, bestHand);
        }

        [Test]
        public void GetHandValue_HandValue_AreEqualTest()
        {
            var cards = new List<Card>();
            cards.Add(new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs });
            cards.Add(new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds });
            cards.Add(new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs });
            cards.Add(new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs });
            cards.Add(new Card() { RankValue = Rank.Four, SuitValue = Suit.Clubs });

            var handValue = Deck.GetHandRanking(cards);

            Assert.AreEqual(Ranking.TwoPair, handValue);
        } 
    }
}