using System;
using System.Collections.Generic;
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

        public static Card[] BestHand(Dictionary<int, Card[]> cardsList)
        {

        }
    }
}
