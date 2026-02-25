using System.Collections.Generic;
using System.Linq;
using Dealer;
using NUnit.Framework;
using static Dealer.Card;
using static Dealer.Deck;

namespace DealerTests
{
    [TestFixture()]
    public class DeckTests
    {
        private Deck _deck;
        
        Card _aceClubs = new Card(Rank.Ace, Suit.Club);
        Card _kingClubs = new Card(Rank.King, Suit.Club);
        
        static readonly Card[] RoyalFlush = new Card[] {
            new Card(Rank.Ace, Suit.Club),
            new Card(Rank.Jack, Suit.Club),
            new Card(Rank.King, Suit.Club),
            new Card(Rank.Ten, Suit.Club),
            new Card(Rank.Queen, Suit.Club)
        };
        static readonly Card[] StraightFlushFive = new Card[] {
                new Card(Rank.Ace, Suit.Spade),
                new Card(Rank.Two, Suit.Spade),
                new Card(Rank.Three, Suit.Spade),
                new Card(Rank.Four, Suit.Spade),
                new Card(Rank.Five, Suit.Spade)
            };
        static readonly Card[] FourOfAKind = new Card[] {
            new Card(Rank.Ace, Suit.Heart),
            new Card(Rank.Ace, Suit.Spade),
            new Card(Rank.Ace, Suit.Club),
            new Card(Rank.Ace, Suit.Diamond),
            new Card(Rank.King, Suit.Spade)
        };
        static readonly Card[] FullHouse = new Card[] {
                new Card(Rank.Ace, Suit.Club),
                new Card(Rank.Ace, Suit.Diamond),
                new Card(Rank.Ace, Suit.Heart),
                new Card(Rank.Jack, Suit.Club),
                new Card(Rank.Jack, Suit.Diamond)
            };
        static readonly Card[] Flush = new Card[] {
            new Card(Rank.Ace, Suit.Club),
            new Card(Rank.Eight, Suit.Club),
            new Card(Rank.Five, Suit.Club),
            new Card(Rank.Four, Suit.Club),
            new Card(Rank.Jack, Suit.Club)
        };
        static readonly Card[] Straight = new Card[] {
            new Card(Rank.Ace, Suit.Club),
            new Card(Rank.Two, Suit.Diamond),
            new Card(Rank.Three, Suit.Diamond),
            new Card(Rank.Four, Suit.Heart),
            new Card(Rank.Five, Suit.Spade)
        };
        static readonly Card[] ThreeOfAKind = new Card[] {
            new Card(Rank.King, Suit.Spade),
            new Card(Rank.King, Suit.Heart),
            new Card(Rank.King, Suit.Diamond),
            new Card(Rank.Jack, Suit.Club),
            new Card(Rank.Nine, Suit.Heart)
        };
        static readonly Card[] TwoPair = new Card[] {
                new Card(Rank.Ace, Suit.Club),
                new Card(Rank.Ace, Suit.Diamond),
                new Card(Rank.Eight, Suit.Club),
                new Card(Rank.Eight, Suit.Club),
                new Card(Rank.Four, Suit.Club)
            };
        static readonly Card[] OnePair = new Card[] {
            new Card(Rank.Nine, Suit.Heart),
            new Card(Rank.Nine, Suit.Diamond),
            new Card(Rank.King, Suit.Diamond),
            new Card(Rank.Jack, Suit.Club),
            new Card(Rank.Queen, Suit.Heart)
        };

        readonly List<Hand> _allHands = new List<Hand>();
        static Hand _royalFlushHand1 = new Hand(1, RoyalFlush.ToList());
        static Hand _straightFlushFiveHand2 = new Hand(2, StraightFlushFive.ToList());
        static Hand _fourOfAKindHand3 = new Hand(3, FourOfAKind.ToList());
        static Hand _fullHouseHand4 = new Hand(4, FullHouse.ToList());
        static Hand _flushHand5 = new Hand(5, Flush.ToList());
        static Hand _straightFiveHand6 = new Hand(6, Straight.ToList());
        static Hand _threeOfAKindHand7 = new Hand(7, ThreeOfAKind.ToList());
        static Hand _twoPairAceEightHand8 = new Hand(8, TwoPair.ToList());
        static Hand _onePairHand9 = new Hand(9, OnePair.ToList());

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
            Assert.IsTrue(_deck.ContainsCard(_aceClubs));
            Assert.AreEqual(Suit.Club, _deck.Cards[0].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[1].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[1].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[2].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[2].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[3].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[3].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[4].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[4].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[5].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[5].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[6].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[6].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[7].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[7].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[8].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[8].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[9].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[9].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[10].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[10].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[11].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[11].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[12].RankValue);
            Assert.AreEqual(Suit.Club, _deck.Cards[12].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[13].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[13].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[14].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[14].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[15].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[15].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[16].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[16].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[17].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[17].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[18].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[18].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[19].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[19].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[20].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[20].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[21].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[21].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[22].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[22].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[23].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[23].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[24].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[24].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[25].RankValue);
            Assert.AreEqual(Suit.Heart, _deck.Cards[25].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[26].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[26].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[27].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[27].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[28].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[28].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[29].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[29].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[30].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[30].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[31].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[31].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[32].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[32].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[33].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[33].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[34].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[34].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[35].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[35].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[36].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[36].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[37].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[37].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[38].RankValue);
            Assert.AreEqual(Suit.Diamond, _deck.Cards[38].SuitValue);
            Assert.AreEqual(Rank.Ace, _deck.Cards[39].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[39].SuitValue);
            Assert.AreEqual(Rank.Two, _deck.Cards[40].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[40].SuitValue);
            Assert.AreEqual(Rank.Three, _deck.Cards[41].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[41].SuitValue);
            Assert.AreEqual(Rank.Four, _deck.Cards[42].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[42].SuitValue);
            Assert.AreEqual(Rank.Five, _deck.Cards[43].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[43].SuitValue);
            Assert.AreEqual(Rank.Six, _deck.Cards[44].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[44].SuitValue);
            Assert.AreEqual(Rank.Seven, _deck.Cards[45].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[45].SuitValue);
            Assert.AreEqual(Rank.Eight, _deck.Cards[46].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[46].SuitValue);
            Assert.AreEqual(Rank.Nine, _deck.Cards[47].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[47].SuitValue);
            Assert.AreEqual(Rank.Ten, _deck.Cards[48].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[48].SuitValue);
            Assert.AreEqual(Rank.Jack, _deck.Cards[49].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[49].SuitValue);
            Assert.AreEqual(Rank.Queen, _deck.Cards[50].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[50].SuitValue);
            Assert.AreEqual(Rank.King, _deck.Cards[51].RankValue);
            Assert.AreEqual(Suit.Spade, _deck.Cards[51].SuitValue);
        }

        [Test]
        public void GetCard_Card_AreEqualTest()
        {
            var card = _deck.GetCard(0);

            Assert.AreEqual(Rank.Ace, card.RankValue);
            Assert.AreEqual(Suit.Club, card.SuitValue);
            Assert.IsNull(_deck.Cards[0]);
        }

        [Test]
        public void ContainsCard_Card_AreEqualTest()
        {
            var expectedCard = new Card(Rank.Ace, Suit.Club);

            Assert.IsTrue(_deck.ContainsCard(expectedCard));
        }

        [Test]
        public void DeckExcludedCards_Card_AreEqualTest()
        {
            var excludedcards = new List<Card>();
            var excludedCard = new Card(Rank.Ace, Suit.Club);
            excludedcards.Add(excludedCard);

            _deck = new Deck(excludedcards);

            Assert.IsFalse(_deck.ContainsCard(excludedCard));
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

        static readonly Hand StraightFlushKing21 = new Hand(21, new Card[] { 
            new Card(Rank.King, Suit.Spade),
            new Card(Rank.Queen, Suit.Spade),
            new Card(Rank.Jack, Suit.Spade),
            new Card(Rank.Ten, Suit.Spade),
            new Card(Rank.Nine, Suit.Spade)
        }.ToList());

        static readonly Hand StraightKingHand22 = new Hand(22, new Card[] {
            new Card(Rank.King, Suit.Club),
            new Card(Rank.Queen, Suit.Diamond),
            new Card(Rank.Jack, Suit.Heart),
            new Card(Rank.Ten, Suit.Spade),
            new Card(Rank.Nine, Suit.Diamond)
        }.ToList());

        static readonly Hand TwoPairQueenEightHand23 = new Hand(23, new Card[] {
            new Card(Rank.Queen, Suit.Club),
            new Card(Rank.Queen, Suit.Diamond),
            new Card(Rank.Eight, Suit.Heart),
            new Card(Rank.Eight, Suit.Spade),
            new Card(Rank.Nine, Suit.Diamond)
        }.ToList());

        static readonly object[] CompareHandsTest = new object[]
        {
            new object [] { StraightFlushKing21, _straightFlushFiveHand2, StraightFlushKing21 },
            new object [] { StraightKingHand22, _straightFiveHand6, StraightKingHand22 },
            new object [] { _twoPairAceEightHand8, _twoPairAceEightHand8, TwoPairQueenEightHand23 }
        };

        [Test, TestCaseSource("CompareHandsTest")]
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
                new Card(Rank.Five, Suit.Club),
                new Card(Rank.Ace, Suit.Diamond),
                new Card(Rank.Ace, Suit.Heart),
                new Card(Rank.Four, Suit.Spade),
                new Card(Rank.Eight, Suit.Club)
            }.ToList();
            var hand = new Hand(1, cards);

            var rankOrder = hand.RankOrder;

            Assert.AreEqual((int)Rank.Ace, rankOrder[0]);
            Assert.AreEqual((int)Rank.Eight, rankOrder[1]);
            Assert.AreEqual((int)Rank.Five, rankOrder[2]);
            Assert.AreEqual((int)Rank.Four, rankOrder[3]);
        }

        [Test]
        public void IsStraight_RanksIsWheel_IsFiveTest()
        {
            var cards = new List<Card>();
            cards.Add(new Card(Rank.Ace, Suit.Club));
            cards.Add(new Card(Rank.Five, Suit.Club));
            cards.Add(new Card(Rank.Four, Suit.Club));
            cards.Add(new Card(Rank.Three, Suit.Club));
            cards.Add(new Card(Rank.Two, Suit.Club));

            var isStraight = GetStraightCards(cards);

            Assert.IsTrue(isStraight);
        }

        [Test]
        public void IsStraight_RanksInSeries_IsFiveTest()
        {
            var cards = new List<Card>();
            cards.Add(new Card(Rank.Ten, Suit.Club));
            cards.Add(new Card(Rank.Jack, Suit.Club));
            cards.Add(new Card(Rank.Queen, Suit.Club));
            cards.Add(new Card(Rank.King, Suit.Club));
            cards.Add(new Card(Rank.Ace, Suit.Club));
            cards.Add(new Card(Rank.Eight, Suit.Club));

            var isStraight = GetStraightCards(cards);

            Assert.IsTrue(isStraight);
        }

        [Test]
        public void IsStraight_RanksNotInSeries_IsEmptyTest()
        {
            var cards = new List<Card>();
            cards.Add(new Card(Rank.Eight, Suit.Club));
            cards.Add(new Card(Rank.Jack, Suit.Club));
            cards.Add(new Card(Rank.Two, Suit.Club));
            cards.Add(new Card(Rank.King, Suit.Club));
            cards.Add(new Card(Rank.Five, Suit.Club));

            var isStraight = GetStraightCards(cards);

            Assert.IsTrue(isStraight);
        }
    }
}