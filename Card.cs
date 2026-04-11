using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _208_Group_Project_Demo
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

    public class Card
    {
        public int value { get; private set; }
        public Suit suit { get; private set; }
        public Rank rank { get; private set; }
        public string id { get; private set; }

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
    }
}
