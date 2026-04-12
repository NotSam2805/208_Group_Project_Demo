using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _208_Group_Project_Demo
{
    public enum BlackjackResult
    {
        PlayerNatural,
        DealerNatural,
        BothNatural,
        PlayerBust,
        DealerBust,
        DealerWins,
        PlayerWins,
        StandOff
    }

    public class Blackjack
    {
        private const int MIN_SHOE = 35;

        public Stack<Card> cardShoe { get; private set; }
        public List<Card> playerHand { get; private set; }
        public List<Card> dealerHand { get; private set; }

        public Blackjack()
        {
            cardShoe = new Stack<Card>();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();
        }

        private void RefillShoe(int nDecks = 6)
        {
            var new_shoe = new List<Card>();
            var deck = Card.GetDeck();
            for (int i = 0; i < nDecks; i++)
            {
                for (int j = 0; j < deck.Length; j++)
                {
                    new_shoe.Add(deck[j]);
                }
            }

            cardShoe = new Stack<Card>(new_shoe.ToArray<Card>());
        }

        private void ShuffleShoe()
        {
            var shoeArray = cardShoe.ToArray();
            shoeArray = Card.ShuffleDeck(shoeArray);
            cardShoe = new Stack<Card>(shoeArray);
        }

        /// <summary>
        /// Deals n cards to either the player or the dealer, specified with toPlayer
        /// </summary>
        /// <param name="n">The number of cards to deal</param>
        /// <param name="toPlayer">If true, cards will be dealt to the player, if false the cards will be dealt to the dealer</param>
        public void DealCards(bool toPlayer, int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                var card = cardShoe.Pop();
                if (toPlayer)
                {
                    playerHand.Add(card);
                }
                else
                {
                    dealerHand.Add(card);
                }
            }
        }

        /// <summary>
        /// Plays a complete game of blackjack between the player and the dealer, handling all game logic and user
        /// interaction.
        /// </summary>
        /// <returns>A BlackjackResult indicating the outcome of the game.</returns>
        public BlackjackResult PlayBlackjack()
        {
            dealerHand.Clear();
            playerHand.Clear();

            if (cardShoe.Count < MIN_SHOE)
            {
                RefillShoe();
            }

            ShuffleShoe();

            DealCards(true, 2);
            DealCards(false, 2);

            int dealerValue = HandValue(dealerHand);
            int playerValue = HandValue(playerHand);

            FrontEnd.Output($"Dealer hand: X, {dealerHand[1].id} ({dealerValue})");
            FrontEnd.Output($"Player hand: {HandToString(playerHand)} ({playerValue})");

            FrontEnd.Output("");

            bool playerNatural = playerValue == 21;
            bool dealerNatural = dealerValue == 21;

            if (playerNatural && dealerNatural)
            {
                FrontEnd.Output("Both have blackjack");
                return BlackjackResult.BothNatural;
            }
            else if (playerNatural)
            {
                FrontEnd.Output("Player Blackjack");
                return BlackjackResult.PlayerNatural;
            }
            else if (dealerNatural)
            {
                FrontEnd.Output("Dealer Blackjack");
                return BlackjackResult.DealerNatural;
            }

            //Players turn
            bool playerStanding = false;
            while (!playerStanding)
            {
                FrontEnd.Output("Hit or stand? (H/S)");
                var playerAction = FrontEnd.Input();

                if (playerAction == "H" || playerAction == "h")
                {
                    DealCards(true);
                    playerValue = HandValue(playerHand);

                    FrontEnd.Output($"Player hand: {HandToString(playerHand)} ({playerValue})");

                    if (playerValue > 21)
                    {
                        FrontEnd.Output("Player bust");
                        FrontEnd.Output($"Dealer: {HandToString(dealerHand)} ({dealerValue})");
                        return BlackjackResult.PlayerBust;
                    }
                    else if (playerValue == 21)
                    {
                        playerStanding = true;
                    }
                }
                else
                {
                    playerStanding = true;
                }
            }

            FrontEnd.Output("");

            //Dealers turn
            bool dealerStanding = false;
            while (!dealerStanding)
            {
                if (dealerValue <= 16)
                {
                    DealCards(false);
                    dealerValue = HandValue(dealerHand);

                    FrontEnd.Output($"Dealer hand: {HandToString(dealerHand)} ({dealerValue})");

                    if (dealerValue > 21)
                    {
                        FrontEnd.Output("Dealer bust");
                        FrontEnd.Output($"Dealer: {HandToString(dealerHand)} ({dealerValue})");
                        return BlackjackResult.DealerBust;
                    }
                    else if (dealerValue == 21)
                    {
                        dealerStanding = true;
                    }
                }
                else
                {
                    dealerStanding = true;
                }
            }

            FrontEnd.Output("");

            if (dealerValue == playerValue)
            {
                FrontEnd.Output("Stand off");
                return BlackjackResult.StandOff;
            }
            if (dealerValue > playerValue)
            {
                FrontEnd.Output("Dealer wins");
                return BlackjackResult.DealerWins;
            }

            FrontEnd.Output("Player wins");
            return BlackjackResult.PlayerWins;
        }


        /// <summary>
        /// Calculates the total value of a hand of cards, treating aces as 1 or 11 to maximize the hand's value without
        /// exceeding 21.
        /// </summary>
        /// <param name="hand">The list of cards whose value is to be calculated.</param>
        /// <returns>The total value of the hand.</returns>
        public static int HandValue(List<Card> hand)
        {
            int value = 0;
            int aceCount = 0;
            foreach (Card card in hand)
            {
                if (card.rank != Rank.Ace)
                {
                    value += card.value;
                }
                else
                {
                    aceCount++;
                }
            }

            for(int i = 0; i < aceCount; i++)
            {
                value += 11;
            }

            while(value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }

        public static string HandToString(List<Card> hand)
        {
            string str = "";
            foreach(Card card in hand)
            {
                str += card.id + ", ";
            }
            return str.Substring(0, str.Length - 2);
        }
    }
}
