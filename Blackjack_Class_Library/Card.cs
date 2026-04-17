using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Class_Library
{
    public enum Suit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    public enum Rank
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        King,
        Queen
    }

    /// <summary>
    /// Represents a playing card with a suit, rank, value, and identifier, and provides methods for creating and
    /// shuffling a standard deck.
    /// </summary>
    public class Card
    {
        public int value { get; private set; }
        public Suit suit { get; private set; }
        public Rank rank { get; private set; }
        public string id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Card class with the specified suit and rank, setting its value and
        /// identifier.
        /// </summary>
        /// <param name="_suit">The suit of the card.</param>
        /// <param name="_rank">The rank of the card.</param>
        public Card(Suit _suit, Rank _rank)
        {
            suit = _suit;
            rank = _rank;
            value = (int)rank + 1;

            id = suit.ToString()[0] + "";
            if (value > 10 || value == 1)
            {
                id += rank.ToString()[0];
            }
            else
            {
                id += value.ToString();
            }

            if (value > 10)
            {
                value = 10;
            }
        }

        /// <summary>
        /// Creates and returns a standard 52-card deck with all suits and ranks.
        /// </summary>
        /// <returns>An array containing all 52 unique cards in a standard deck.</returns>
        public static Card[] GetDeck()
        {
            var deck = new Card[52];

            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    deck[count] = new Card((Suit)i, (Rank)j);
                    count++;
                }
            }

            return deck;
        }

        /// <summary>
        /// Randomizes the order of cards in the specified deck.
        /// </summary>
        /// <param name="deck">The array of cards to shuffle.</param>
        /// <returns>The shuffled array of cards.</returns>
        public static Card[] ShuffleDeck(Card[] deck)
        {
            var rnd = new Random();
            rnd.Shuffle<Card>(deck);
            return deck;
        }
    }
}
