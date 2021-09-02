using System.Collections.Generic;
using System.Linq;
using static Dealer.Card;

namespace Dealer
{
    public class Deck
    {
        Suit[] _suits = new Suit[] { Suit.Clubs, Suit.Hearts, Suit.Diamonds, Suit.Spades };
        Rank[] _ranks = new Rank[] { 
            Rank.Ace, 
            Rank.Two,
            Rank.Three,
            Rank.Four,
            Rank.Five, 
            Rank.Six, 
            Rank.Seven, 
            Rank.Eight,
            Rank.Nine, 
            Rank.Ten, 
            Rank.Jack,
            Rank.Queen,
            Rank.King 
        };

        public enum Ranking
        {
            None = 0,
            OnePair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            Straight = 4,
            Flush = 5,
            FullHouse = 6,
            FourOfAKind = 7,
            StraightFlush = 8,
            RoyalFlush = 9
        }

        public List<Card> Cards { get; set; } = new List<Card>();

        public Deck(List<Card> excludedCards = null)
        {
            if (excludedCards == null)
                CreateDeck(new List<Card>());
            else
                CreateDeck(excludedCards);
        }

        private void CreateDeck(List<Card> excludedCards)
        {
            foreach (var suit in _suits)
                foreach (var rank in _ranks)
                    if (!ContainsCards(excludedCards, new Card() { RankValue = rank, SuitValue = suit }))
                        Cards.Add(new Card() { RankValue = rank, SuitValue = suit });
        }

        public Card GetCard(int slot)
        {
            var card = Cards[slot];
            Cards[slot] = null;

            return card;
        }

        public Card GetRandomCard()
        {
            Card card = null;
            while (card == null && Cards.Count > 0)
                card = GetCard(RandomNumberGeneratorCustom.GetNumber(Cards.Count) - 1);

            return card;
        }

        public static bool ContainsCards(List<Card> cardList, Card cardMatch)
        {
            foreach (var card in cardList)
                if (card.SuitValue == cardMatch.SuitValue && card.RankValue == cardMatch.RankValue) return true;

            return false;
        }

        public static Hand BestHand(List<Hand> hands)
        {
            Hand bestHand = null;
            foreach (var hand in hands)
            {
                if (bestHand == null)
                {
                    bestHand = hand;
                }
                else
                {
                    if (GetHandRanking(hand).Ranking > bestHand.Ranking)
                        bestHand = GetHandRanking(hand);
                    else if (GetHandRanking(hand).Ranking == bestHand.Ranking)
                        bestHand = CompareEqualHands(bestHand, hand);
                }
            }

            return bestHand;
        }

        public static Hand CompareEqualHands(Hand firstHand, Hand secondHand)
        {
            for (var i = 0; i < firstHand.RankOrder.Count; i++)
            {
                if ((int)firstHand.RankOrder[i] > (int)secondHand.RankOrder[i])
                    return firstHand;
                if ((int)firstHand.RankOrder[i] < (int)secondHand.RankOrder[i])
                    return secondHand;
            }

            return null;
        }

        public static Hand GetHandRanking(Hand hand)
        {
            hand.Ranking = Ranking.None;
            hand.RankOrder = new List<int>();

            
            if (IsFlush(hand.Cards) && IsStraight(hand.Cards))
            {
                hand.Ranking = Ranking.StraightFlush;
            }
            else if (IsFlush(hand.Cards))
            {
                hand.Ranking = Ranking.Flush;
            }
            else if (IsStraight(hand.Cards))
            {
                hand.Ranking = Ranking.Straight;
            }

            return hand;
        }

        public static List<int> RankOrderTwoPair(Dictionary<Rank, int> ranks)
        {
            var rankOrder = (Dictionary<Rank, int>)ranks
                    .OrderByDescending(r => r.Value)
                    .ThenBy(r => r.Key);
            var rankOrderList = rankOrder.Keys.ToList();
            var rankOrderValue = new List<int>();
            foreach (var rank in rankOrderList)
                rankOrderValue.Add((int)rank);

            return rankOrderValue;
        }

        public static bool IsFlush(List<Card> cards)
        {
            var matchingSuits = new Dictionary<Suit, int>();

            foreach (var card in cards)
            {
                if (matchingSuits.ContainsKey(card.SuitValue))
                    matchingSuits[card.SuitValue] = matchingSuits[card.SuitValue] + 1;
                else
                    matchingSuits.Add(card.SuitValue, 1);
            }

            foreach (var matchingSuit in matchingSuits)
                if (matchingSuit.Value == 5) return true;

            return false;
        }

        public static bool IsStraight(List<Card> cards)
        {
            var sortedCards = cards.OrderByDescending(c => (int)c.RankValue);
            Card previousCard = null;

            foreach (var sortedCard in sortedCards)
            {
                if (previousCard != null)
                    if (previousCard.RankValue != sortedCard.RankValue + 1 &&
                        !(previousCard.RankValue == Rank.Ace && 
                        sortedCard.RankValue == Rank.Five))
                    {
                        return false;
                    }

                previousCard = sortedCard;
            }

            return true;
        }

        private static int HighestCard(List<Card> cards)
        {
            var sortedCards = cards.OrderByDescending(c => c.RankValue);

            var hand = new List<Card>();
            var score = 0;
            for (var i = 0; i < 5; i++)
            {
                score += (int)sortedCards.ElementAt(i).RankValue;
                hand.Add(sortedCards.ElementAt(i));
            }

            return score;
        }
    }
}
