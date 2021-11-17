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
                ShuffleDeck(new List<Card>());
            else
                ShuffleDeck(excludedCards);
        }

        private void ShuffleDeck(List<Card> excludedCards)
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

        public Hand BestHand(List<Hand> hands)
        {
            Hand bestHand = null;
            foreach (var hand in hands)
            {
                if (bestHand != null)
                {
                    if (hand.Ranking > bestHand.Ranking)
                        bestHand = hand;
                    else if (hand.Ranking == bestHand.Ranking)
                        bestHand = CompareEqualHands(bestHand, hand);
                }
                else
                {
                    bestHand = hand;
                }
            }

            return bestHand;
        }

        public static Hand CompareEqualHands(Hand firstHand, Hand secondHand)
        {
            for (var i = 0; i < firstHand.RankOrder.Count; i++)
            {
                if (firstHand.RankOrder[i] > secondHand.RankOrder[i])
                    return firstHand;
                if (firstHand.RankOrder[i] < secondHand.RankOrder[i])
                    return secondHand;
            }

            return null;
        }

        public static List<Card> GetStraightCards(List<Card> cards)
        {
            List<Card> straightcards = new List<Card>();
            var sortedcards = cards.OrderByDescending(c => c.RankValue);
            Card previouscard = null;
            for (var i = 0; i < sortedcards.Count(); i++)
                if (previouscard != null && previouscard.RankValue + 1 == sortedcards.ElementAt(i).RankValue)
                    straightcards.Add(sortedcards.ElementAt(i));

            return straightcards;
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
