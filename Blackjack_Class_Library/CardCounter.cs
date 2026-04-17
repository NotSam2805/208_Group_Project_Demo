using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Class_Library
{
    /// <summary>
    /// Provides card counting functionality for a Blackjack game, maintaining running and true counts based on cards
    /// dealt to the player and dealer.
    /// </summary>
    public class CardCounter
    {
        /// <summary>
        /// Represents the current Blackjack game instance.
        /// </summary>
        public Blackjack game;

        // The counts
        private int runningCount;
        private double trueCount;

        // For checking if counts need to be update due to more cards been dealt since last check
        private int lastDealerHandCount;
        private int lastPlayerHandCount;

        private int lastShoeCount;

        public double decksLeft { get; private set; }
        
        // Array containing card ranks.
        // All ranks in Los increase the count.
        // All ranks in Noughts do not change the count.
        // All ranks in His decrease the count.
        private Rank[] Los = { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six };
        private Rank[] Noughts = { Rank.Seven, Rank.Eight, Rank.Nine};
        private Rank[] His = { Rank.Ace, Rank.Ten, Rank.Jack, Rank.King, Rank.Queen };

        /// <summary>
        /// Initializes a new instance of the CardCounter class with the specified Blackjack game.
        /// </summary>
        /// <param name="_game">The Blackjack game instance to associate with the card counter.</param>
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

            }

            var dealerHand = game.LookAtDealerHand();

            if (dealerHand.Count > lastDealerHandCount)
            {
                for (int i = lastDealerHandCount; i < dealerHand.Count; i++)
                {
                    CountCard(dealerHand[i]);
                }

            }

            lastShoeCount = game.ShoeCount();

            lastPlayerHandCount = game.playerHand.Count;
            lastDealerHandCount = dealerHand.Count;

            return runningCount;
        }

        /// <summary>
        /// Calculates and returns the true count by dividing the running count by the number of decks remaining in the
        /// shoe.
        /// </summary>
        /// <returns>The current true count as an integer.</returns>
        public double GetTrueCount()
        {
            decksLeft = game.ShoeCount() / 52d;
            trueCount = runningCount / decksLeft;
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
                runningCount += 1;
                return;
            }
            if (Noughts.Contains(card.rank))
            {
                return;
            }
            if (His.Contains(card.rank))
            {
                runningCount -= 1;
                return;
            }
        }
    }
}
