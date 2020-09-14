using System.Collections.Generic;
using System.Linq;
using static Dealer.Card;
using static Dealer.Deck;

namespace Dealer
{
    public class Hand
    {
        public int PlayerId { get; set; }
        public List<Card> Cards { get; set; }
        public Ranking Ranking => GetHandRanking();
        public List<int> RankOrder => GetRankOrder();
        public Dictionary<int, int> RankCounts => GetRankCounts();
        public Dictionary<int, int> SuitCounts => GetSuitCounts();

        public Hand(int playerId, List<Card> cards)
        {
            PlayerId = playerId;
            Cards = cards;
        }

        private Dictionary<int, int> GetRankCounts()
        {
            var rankValues = new List<int>();
            foreach (var card in Cards)
                rankValues.Add((int)card.RankValue);

            return GetCardCounts(rankValues);
        }

        private Dictionary<int, int> GetSuitCounts()
        {
            var suitValues = new List<int>();
            foreach (var card in Cards)
                suitValues.Add((int)card.SuitValue);

            return GetCardCounts(suitValues);
        }

        private Dictionary<int, int> GetCardCounts(List<int> cardValues)
        {
            var cardCounts = new Dictionary<int, int>();
            foreach (var cardValue in cardValues)
            {
                if (cardCounts.ContainsKey(cardValue))
                    cardCounts[cardValue]++;
                else
                    cardCounts.Add(cardValue, 1);
            }

            return cardCounts;
        }

        private List<int> GetRankOrder()
        {
            var keys = new List<int>();
            var orderedRanks = RankCounts.OrderByDescending(r => r.Value).ThenByDescending(r => r.Key);
            foreach (var orderedRank in orderedRanks)
                keys.Add(orderedRank.Key);
            if (IsWheel())
            {
                keys.Remove(14);
                keys.Add(1);
            }

            return keys;
        }

        private Ranking GetHandRanking()
        {

            var handRanking = Ranking.None;

            if (RankCounts.Count == 5)
            {
                var sortedCards = Cards.OrderByDescending(c => (int)c.RankValue);
                var firstCard = (int)sortedCards.ElementAt(0).RankValue;
                
                var currentRankValue = firstCard;
                for (var i = 0; i < 5; i++)
                {
                    var currentRank = sortedCards.ElementAt(i).RankValue;
                    if (currentRankValue != (int)currentRank)
                        break;
                    currentRankValue--;

                    if (i == 4)
                        handRanking = Ranking.Straight;
                    else
                        handRanking = Ranking.None;
                }
                if (IsWheel())
                    handRanking = Ranking.Straight;
                if (SuitCounts.Count == 1 && handRanking == Ranking.Straight)
                {
                    if (sortedCards.ElementAt(0).RankValue == Rank.Ace &&
                        sortedCards.ElementAt(4).RankValue == Rank.Ten)
                        handRanking = Ranking.RoyalFlush;
                    else
                        handRanking = Ranking.StraightFlush;
                }
                else if (SuitCounts.Count == 1)
                {
                    handRanking = Ranking.Flush;
                }
            }
            if (RankCounts.Count == 4)
                handRanking = Ranking.OnePair;
            if (RankCounts.Count == 3)
            {
                if (RankCounts.ContainsValue(2))
                    handRanking = Ranking.TwoPair;
                else if (RankCounts.ContainsValue(3))
                    handRanking = Ranking.ThreeOfAKind;
            }
            if (RankCounts.Count == 2)
            {
                if (RankCounts.ContainsValue(2) && RankCounts.ContainsValue(3))
                    handRanking = Ranking.FullHouse;
                else
                    handRanking = Ranking.FourOfAKind;
            }

            return handRanking;
        }

        private bool IsWheel()
        {
            var sortedCards = Cards.OrderByDescending(c => (int)c.RankValue);
            if (sortedCards.ElementAt(0).RankValue == Rank.Ace)
            {
                if (sortedCards.ElementAt(1).RankValue == Rank.Five &&
                    sortedCards.ElementAt(2).RankValue == Rank.Four &&
                    sortedCards.ElementAt(3).RankValue == Rank.Three &&
                    sortedCards.ElementAt(4).RankValue == Rank.Two)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
