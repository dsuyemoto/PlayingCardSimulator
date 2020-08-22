using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            None =0,
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
                if (bestHand != null)
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

            var rankMatches = new Dictionary<Rank, int>();
            var suitMatches = new Dictionary<Suit, int>();

            foreach (var card in hand.Cards)
            {
                if (rankMatches.ContainsKey(card.RankValue))
                    rankMatches[card.RankValue]++;
                else
                    rankMatches.Add(card.RankValue, 1);

                if (suitMatches.ContainsKey(card.SuitValue))
                    suitMatches[card.SuitValue]++;
                else
                    suitMatches.Add(card.SuitValue, 1);
            }

            if (rankMatches.Count == 5)
            {
                var sortedCards = hand.Cards.OrderByDescending(c => (int)c.RankValue);
                var firstCard = (int)sortedCards.ElementAt(0).RankValue;
                if (firstCard == (int)Rank.Ace)
                {
                    if (sortedCards.ElementAt(1).RankValue == Rank.Five &&
                        sortedCards.ElementAt(2).RankValue == Rank.Four &&
                        sortedCards.ElementAt(3).RankValue == Rank.Three &&
                        sortedCards.ElementAt(4).RankValue == Rank.Two)
                    {
                        hand.RankOrder = new int[] { 5,4,3,2,1 }.ToList();
                        hand.Ranking = Ranking.Straight;
                    }
                }

                var currentRankValue = firstCard;
                var rankOrder = new List<int>();
                for (var i = 0; i < 5; i++)
                {
                    var currentRank = sortedCards.ElementAt(i).RankValue;
                    if (currentRankValue != (int)currentRank)
                        break;
                    rankOrder.Add(currentRankValue);
                    currentRankValue--;
                    
                    if (i == 5)
                    {
                        hand.RankOrder = rankOrder;
                        hand.Ranking = Ranking.Straight;
                    }
                }
            }

            if (rankMatches.Count == 4)
            {
                hand.RankOrder = RankOrderTwoPair(rankMatches);
                hand.Ranking = Ranking.OnePair;
            }
            if (rankMatches.Count == 3)
            {
                if (rankMatches.ContainsValue(2))
                {

                    hand.Ranking = Ranking.TwoPair;
                }
                else if (rankMatches.ContainsValue(3))
                    hand.Ranking = Ranking.ThreeOfAKind;
            }
            if (rankMatches.Count == 2)
            {
                if (rankMatches.ContainsValue(2) && rankMatches.ContainsValue(3))
                    hand.Ranking = Ranking.FullHouse;
                else
                    hand.Ranking = Ranking.FourOfAKind;
            }
            if (suitMatches.Count == 1 && hand.Ranking == Ranking.Straight)
            {
                var sortedCards = hand.Cards.OrderByDescending(c => c.RankValue);
                if (sortedCards.ElementAt(0).RankValue == Rank.Ace && 
                    sortedCards.ElementAt(4).RankValue == Rank.Ten)
                    hand.Ranking = Ranking.RoyalFlush;
                else
                    hand.Ranking = Ranking.StraightFlush;
            }
            if (suitMatches.Count == 1)
                hand.Ranking = Ranking.Flush;

            rankMatches.OrderByDescending(r => r.Key);
            var rankKeys = rankMatches.Keys.ToList();
            for (var i = 0; i < rankKeys.Count; i++)
                hand.RankOrder[i] = (int)rankKeys[i];

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
