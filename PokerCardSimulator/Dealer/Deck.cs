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
            foreach (var hand in hands)
            {
                var handIndex = hands.FindIndex(h => h.PlayerId == hand.PlayerId);
                hands[handIndex].Score = CalculateScore(hand.Cards);
            }

            return hands.OrderByDescending(h => h.Score).First();
        }

        public static int CalculateScore(List<Card> cards)
        {
            return -1;
        }

        public static Ranking GetHandRanking(List<Card> cards)
        {
            var rankMatches = new Dictionary<Rank, int>();
            var suitMatches = new Dictionary<Suit, int>();

            foreach (var card in cards)
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

            if (rankMatches.Count == 4)
                return Ranking.OnePair;
            if (rankMatches.Count == 3)
            {
                if (rankMatches.ContainsValue(2))
                    return Ranking.TwoPair;
                else if (rankMatches.ContainsValue(3))
                    return Ranking.ThreeOfAKind;
            }
            if (rankMatches.Count == 2)
            {
                if (rankMatches.ContainsValue(2) && rankMatches.ContainsValue(3))
                    return Ranking.FullHouse;
                else
                    return Ranking.FourOfAKind;
            }

            if (suitMatches.Count == 1 && IsStraight(cards))
            {
                var sortedCards = cards.OrderByDescending(c => c.RankValue);
                if (cards.ElementAt(0).RankValue == Rank.Ace && 
                    cards.ElementAt(4).RankValue == Rank.Ten)
                    return Ranking.RoyalFlush;
                else
                    return Ranking.StraightFlush;
            }
            if (suitMatches.Count == 1)
                return Ranking.Flush;
            if (IsStraight(cards))
                return Ranking.Straight;

            return Ranking.None;
        }

        private static bool IsStraight(List<Card> cards)
        {
            var sortedCards = cards.OrderByDescending(c => c.RankValue);

            var currentIndex = (int)sortedCards.ElementAt(0).RankValue;
            for (var i = 0; i < 5; i++)
            {
                if (currentIndex != (int)sortedCards.ElementAt(i).RankValue)
                    return false;
                currentIndex++;
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
