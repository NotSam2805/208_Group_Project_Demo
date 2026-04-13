using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _208_Group_Project_Demo
{
    /// <summary>
    /// Calculates counts according to a HiLo card counting system
    /// </summary>
    public class CardCounter
    {
        public Blackjack game;
        private int runningCount;
        private double trueCount;

        private int lastDealerHandCount;
        private int lastPlayerHandCount;

        public int lastShoeCount;

        // Cards in 'Los' are counted as -1, 'Noughts' as 0, 'His' as 1
        private Rank[] Los = { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six };
        private Rank[] Noughts = { Rank.Seven, Rank.Eight, Rank.Nine};
        private Rank[] His = { Rank.Ace, Rank.Ten, Rank.Jack, Rank.King, Rank.Queen };

        public CardCounter(Blackjack _game)
        {
            game = _game;
            runningCount = 0;
            trueCount = 0;

            lastDealerHandCount = 0;
            lastPlayerHandCount = 0;

            lastShoeCount = 0;
        }

        /// <summary>
        /// Calculates and returns the current running count based on the cards dealt to the player and dealer,
        /// resetting the count if the card shoe has been refilled.
        /// </summary>
        /// <returns>The current running count after processing new cards.</returns>
        public int GetRunningCount()
        {
            if (game.ShoeCount() > lastShoeCount)
            {
                //Card shoe has been refilled
                runningCount = 0;

                lastPlayerHandCount = 0;
                lastDealerHandCount = 0;
            }

            if (game.playerHand.Count > lastPlayerHandCount)
            {
                for (int i = lastPlayerHandCount; i < game.playerHand.Count; i++)
                {
                    CountCard(game.playerHand[i]);
                }

                lastPlayerHandCount = game.playerHand.Count;
            }

            var dealerHand = game.LookAtDealerHand();

            if (dealerHand.Count > lastDealerHandCount)
            {
                for (int i = lastDealerHandCount; i < dealerHand.Count; i++)
                {
                    CountCard(dealerHand[i]);
                }

                lastDealerHandCount = dealerHand.Count;
            }

            lastShoeCount = game.ShoeCount();
            return runningCount;
        }

        /// <summary>
        /// Calculates and returns the true count by dividing the running count by the number of decks remaining in the
        /// shoe.
        /// </summary>
        /// <returns>The current true count as an integer.</returns>
        public double GetTrueCount()
        {
            int nDecksLeft = game.ShoeCount() / 52;
            trueCount = (double)runningCount / (double)nDecksLeft;
            return trueCount;
        }

        /// <summary>
        /// Adjusts the running count based on the rank of the specified card using predefined rank groups.
        /// </summary>
        /// <param name="card">The card whose rank is evaluated to update the running count.</param>
        private void CountCard(Card card)
        {
            if (Los.Contains(card.rank))
            {
                runningCount -= 1;
                return;
            }
            if (Noughts.Contains(card.rank))
            {
                return;
            }
            if (His.Contains(card.rank))
            {
                runningCount += 1;
                return;
            }
        }
    }
}
