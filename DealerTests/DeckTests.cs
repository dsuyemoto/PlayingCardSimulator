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
        static Card[] _royalFlush = new Card[] {
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.King, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Ten, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Clubs }
        };
        static Card[] _straightFlushFive = new Card[] {
                new Card() {  RankValue = Rank.Ace, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Two, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Three, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Four, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Five, SuitValue = Suit.Spades }
            };
        static Card[] _fourOfAKind = new Card[] {
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.King, SuitValue = Suit.Spades }
        };
        static Card[] _fullHouse = new Card[] {
                new Card() {  RankValue = Rank.Ace, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Hearts },
                new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Jack, SuitValue = Suit.Diamonds }
            };
        static Card[] _flush = new Card[] {
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Five, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Four, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs }
        };
        static Card[] _straight = new Card[] {
            new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Two, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Three, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Four, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.Five, SuitValue = Suit.Spades }
        };
        static Card[] _threeOfAKind = new Card[] {
            new Card() { RankValue = Rank.King, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.King, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.King, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Hearts }
        };
        static Card[] _twoPair = new Card[] {
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds },
                new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Four, SuitValue = Suit.Clubs }
            };
        static Card[] _onePair = new Card[] {
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.King, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Hearts }
        };
        List<Hand> _allHands = new List<Hand>();
        static Hand _royalFlushHand1 = new Hand(1, _royalFlush.ToList());
        static Hand _straightFlushFiveHand2 = new Hand(2, _straightFlushFive.ToList());
        static Hand _fourOfAKindHand3 = new Hand(3, _fourOfAKind.ToList());
        static Hand _fullHouseHand4 = new Hand(4, _fullHouse.ToList());
        static Hand _flushHand5 = new Hand(5, _flush.ToList());
        static Hand _straightFiveHand6 = new Hand(6, _straight.ToList());
        static Hand _threeOfAKindHand7 = new Hand(7, _threeOfAKind.ToList());
        static Hand _twoPairAceEightHand8 = new Hand(8, _twoPair.ToList());
        static Hand _onePairHand9 = new Hand(9, _onePair.ToList());

        public DeckTests()
        {
            _allHands.Add(_royalFlushHand1);
            _allHands.Add(_straightFlushFiveHand2);
            _allHands.Add(_fullHouseHand4);
            _allHands.Add(_twoPairAceEightHand8);
            
        }

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

            Assert.IsTrue(ContainsCards(_deck.Cards, expectedCard));
        }

        [Test]
        public void DeckExcludedCards_Card_AreEqualTest()
        {
            var cards = new List<Card>();
            var excludedCard = new Card() { RankValue = Rank.Ace, SuitValue = Suit.Clubs };
            cards.Add(excludedCard);

            _deck = new Deck(cards);

            Assert.IsFalse(ContainsCards(_deck.Cards, excludedCard));
        }

        static object[] _getHandRankingTest = new object[] {
                new object[] { Ranking.RoyalFlush, _royalFlushHand1 },
                new object[] { Ranking.StraightFlush, _straightFlushFiveHand2 },
                new object[] { Ranking.FourOfAKind, _fourOfAKindHand3 },
                new object[] { Ranking.FullHouse, _fullHouseHand4 },
                new object[] { Ranking.Flush, _flushHand5 },
                new object[] { Ranking.Straight, _straightFiveHand6 },
                new object[] { Ranking.ThreeOfAKind, _threeOfAKindHand7 },
                new object[] { Ranking.TwoPair, _twoPairAceEightHand8 },
                new object[] { Ranking.OnePair, _onePairHand9 }
            };

        [Test, TestCaseSource("_getHandRankingTest")]
        public void GetHandRanking_Ranking_AreEqualTest(Ranking rankingMatch, Hand hand)
        {
            var ranking = hand.Ranking;

            Assert.AreEqual(rankingMatch, ranking);
        }

        [Test]
        public void BestHands_Cards_InOrderTest()
        {
            var hands = new List<Hand>();
            hands.Add(_fullHouseHand4);
            hands.Add(_royalFlushHand1);
            hands.Add(_straightFlushFiveHand2);

            var bestHand = _deck.BestHand(hands);

            Assert.AreEqual(Ranking.RoyalFlush, bestHand.Ranking);
        }

        static Hand straightFlushKing21 = new Hand(21, new Card[] { 
            new Card() { RankValue = Rank.King, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Ten, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Spades }
        }.ToList());

        static Hand straightKingHand22 = new Hand(22, new Card[] {
            new Card() { RankValue = Rank.King, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Jack, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.Ten, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Diamonds }
        }.ToList());

        static Hand twoPairQueenEightHand23 = new Hand(23, new Card[] {
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Clubs },
            new Card() { RankValue = Rank.Queen, SuitValue = Suit.Diamonds },
            new Card() { RankValue = Rank.Eight, SuitValue = Suit.Hearts },
            new Card() { RankValue = Rank.Eight, SuitValue = Suit.Spades },
            new Card() { RankValue = Rank.Nine, SuitValue = Suit.Diamonds }
        }.ToList());

        static object[] compareHandsTest = new object[]
        {
            new object [] { straightFlushKing21, _straightFlushFiveHand2, straightFlushKing21 },
            new object [] { straightKingHand22, _straightFiveHand6, straightKingHand22 },
            new object [] { _twoPairAceEightHand8, _twoPairAceEightHand8, twoPairQueenEightHand23 }
        };

        [Test, TestCaseSource("compareHandsTest")]
        public void CompareHands_Hands_AreEqualTest(Hand expectedHand, Hand firstHand, Hand secondHand)
        {
            var bestHand = CompareEqualHands(firstHand, secondHand);

            Assert.AreEqual(expectedHand, bestHand);
        }

        [Test]
        public void RankOrderTwoPair_Ranks_AreEqualTest()
        {
            var cards = new Card[]
            {
                new Card() { RankValue = Rank.Five, SuitValue = Suit.Clubs },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Diamonds },
                new Card() { RankValue = Rank.Ace, SuitValue = Suit.Hearts },
                new Card() { RankValue = Rank.Four, SuitValue = Suit.Spades },
                new Card() { RankValue = Rank.Eight, SuitValue = Suit.Clubs }
            }.ToList();
            var hand = new Hand(1, cards);

            var rankOrder = hand.RankOrder;

            Assert.AreEqual((int)Rank.Ace, rankOrder[0]);
            Assert.AreEqual((int)Rank.Eight, rankOrder[1]);
            Assert.AreEqual((int)Rank.Five, rankOrder[2]);
            Assert.AreEqual((int)Rank.Four, rankOrder[3]);
        }
    }
}