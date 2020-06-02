using System;
using System.Collections.Generic;
using System.Text;
using static CardDeck.Card;

namespace CardDeck
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

        public Deck()
        {
            CreateDeck();
        }

        public void CreateDeck()
        {
            foreach (var suit in _suits)
                foreach (var rank in _ranks)
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
                card = GetCard(RandomNumberGeneratorCustom.GetNumber(Cards.Count)-1);

            return card;
        }
    }
}
