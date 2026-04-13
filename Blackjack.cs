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

    public enum BlackjackState
    {
        NoGame,
        PlayerTurn,
        DealerTurn,
        GameEnd
    }

    public enum BlackjackAction
    {
        Hit,
        Stand,
        DoubleDown
    }

    /// <summary>
    /// Represents a game of blackjack, managing the card shoe, player and dealer hands, and game logic.
    /// </summary>
    public class Blackjack
    {
        private const int MIN_SHOE = 35;
        private const int INITIAL_CREDIT = 5000;

        private Stack<Card> cardShoe;

        public List<Card> playerHand { get; private set; }
        public int playerValue { get; private set; }

        //Values used for handling bets and player's credit
        public double? playerBet { get; private set; }
        public double playerCredit { get; private set; }

        private List<Card> dealerHand;
        private int dealerValue;
        
        public BlackjackState currentState { get; private set; }

        public Blackjack()
        {
            cardShoe = new Stack<Card>();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            playerCredit = INITIAL_CREDIT;
            playerBet = null;

            currentState = BlackjackState.NoGame;
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
        /// <param name="toHand">The hand the cards are dealt to</param>
        /// <returns>The hand with cards dealt to it</returns>
        public List<Card> DealCards(List<Card> toHand, int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                var card = cardShoe.Pop();
                toHand.Add(card);
            }

            return toHand;
        }

        /// <summary>
        /// Plays a complete game of blackjack between the player and the dealer, handling all game logic and user
        /// interaction. Use of this function is not recommended, use StartBlackjack to start a game.
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

            playerHand = DealCards(playerHand, 2);
            dealerHand = DealCards(dealerHand, 2);

            dealerValue = HandValue(dealerHand);
            playerValue = HandValue(playerHand);

            var tempHand = dealerHand;
            tempHand.Remove(tempHand[0]);
            FrontEnd.Output($"Dealer hand: X, {dealerHand[1].id} ({HandValue(tempHand)})");
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
                    playerHand = DealCards(playerHand);
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
                    dealerHand = DealCards(dealerHand);
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

        public void PlaceBet(double bet)
        {
            if (currentState != BlackjackState.NoGame)
            {
                throw new Exception("Cannot place a bet during play");
            }

            if (bet <= 0)
            {
                throw new Exception("Cannot place bet of 0 or less");
            }

            playerCredit -= bet;
            playerBet = bet;
        }

        public void ShowPlayerCredit()
        {
            FrontEnd.Output($"Credit: {playerCredit}");
            if (playerBet != null)
            {
                FrontEnd.Output($"Current bet: {playerBet}");
            }
        }

        /// <summary>
        /// Initializes and starts a new game of blackjack, dealing cards to the player and dealer and updating the game
        /// state.
        /// </summary>
        public void StartBlackjack()
        {
            if (currentState != BlackjackState.NoGame)
            {
                FrontEnd.Output("Cannot start new game during play");
                return;
            }

            dealerHand.Clear();
            playerHand.Clear();

            if (cardShoe.Count < MIN_SHOE)
            {
                RefillShoe();
            }

            ShuffleShoe();

            playerHand = DealCards(playerHand, 2);
            dealerHand = DealCards(dealerHand, 2);

            dealerValue = HandValue(dealerHand);
            playerValue = HandValue(playerHand);

            var tempHand = new List<Card>(dealerHand);
            tempHand.Remove(tempHand[0]);
            FrontEnd.Output($"Dealer hand: X, {dealerHand[1].id} ({HandValue(tempHand)})");
            FrontEnd.Output($"Player hand: {HandToString(playerHand)} ({playerValue})");

            FrontEnd.Output("");

            if(playerValue == 21 || dealerValue == 21)
            {
                currentState = BlackjackState.GameEnd;
                return;
            }

            currentState = BlackjackState.PlayerTurn;
        }

        public bool CanDoubleDown()
        {
            return (currentState == BlackjackState.PlayerTurn) && (playerHand.Count == 2) && (playerBet != null);
        }

        /// <summary>
        /// Processes the player's action during their turn in a game of Blackjack.
        /// </summary>
        /// <param name="playerAction">The action chosen by the player, such as Hit or Stand.</param>
        public void PlayerTurn(BlackjackAction playerAction)
        {
            FrontEnd.Output("");
            if (currentState != BlackjackState.PlayerTurn)
            {
                FrontEnd.Output("It is not the player's turn");
                return;
            }

            if (playerAction == BlackjackAction.DoubleDown)
            {
                if (playerBet == null)
                {
                    throw new Exception("Cannot double down when no bet was placed");
                }

                if (playerHand.Count > 2)
                {
                    throw new Exception("Can only double down on first turn");
                }

                if (!CanDoubleDown())
                {
                    throw new Exception("Cannot double down for unknown reason");
                }

                //Double the bet
                playerCredit -= (double)playerBet;
                playerBet += playerBet;

                //Committed to one hit then stand
                PlayerHit();
                //If PlayerHit didn't change the state, need to ensure the player has no more turns
                if (currentState == BlackjackState.PlayerTurn)
                {
                    currentState = BlackjackState.DealerTurn;
                }
            }

            if (playerAction == BlackjackAction.Hit)
            {
                PlayerHit();
            }

            if (playerAction == BlackjackAction.Stand)
            {
                currentState = BlackjackState.DealerTurn;
                return;
            }
        }

        /// <summary>
        /// Deals a card to the player, updates the player's hand value, displays the hand, and transitions the game
        /// state based on the new hand value.
        /// </summary>
        private void PlayerHit()
        {
            playerHand = DealCards(playerHand);
            playerValue = HandValue(playerHand);
            FrontEnd.Output($"Player hand: {HandToString(playerHand)} ({playerValue})");

            if (playerValue > 21)
            {
                currentState = BlackjackState.GameEnd;
            }
            else if (playerValue == 21)
            {
                currentState = BlackjackState.DealerTurn;
            }

        }

        /// <summary>
        /// Executes the dealer's turn in the Blackjack game, updating the dealer's hand and game state according to
        /// standard rules.
        /// </summary>
        public void DealerTurn()
        {
            FrontEnd.Output("");
            if (currentState != BlackjackState.DealerTurn)
            {
                FrontEnd.Output("Not the dealer's turn");
                return;
            }

            if (dealerValue > 16)
            {
                currentState = BlackjackState.GameEnd;
                return;
            }
            else
            {
                dealerHand = DealCards(dealerHand);
                dealerValue = HandValue(dealerHand);

                FrontEnd.Output($"Dealer hand: {HandToString(dealerHand)} ({dealerValue})");

                if (dealerValue >= 21)
                {
                    currentState = BlackjackState.GameEnd;
                    return;
                }
            }
        }

        /// <summary>
        /// Ends the current blackjack game, determines the outcome based on player and dealer hands, and outputs the
        /// result.
        /// </summary>
        /// <returns>A BlackjackResult value indicating the outcome of the game.</returns>
        public BlackjackResult EndGame()
        {
            FrontEnd.Output("");
            if (currentState != BlackjackState.GameEnd)
            {
                throw (new Exception("Cannot end game yet"));
            }

            FrontEnd.Output($"Dealer hand: X, {HandToString(dealerHand)} ({dealerValue})");
            FrontEnd.Output($"Player hand: {HandToString(playerHand)} ({playerValue})");

            currentState = BlackjackState.NoGame;

            if (playerValue > 21)
            {
                FrontEnd.Output("Player bust");

                playerBet = null;

                return BlackjackResult.PlayerBust;
            }
            if (dealerValue > 21)
            {
                FrontEnd.Output("Dealer bust");

                if (playerBet != null)
                {
                    playerCredit += 2d * (double)playerBet;
                    playerBet = null;
                }

                return BlackjackResult.DealerBust;
            }

            if (playerHand.Count == 2 || dealerHand.Count == 2)
            {
                bool playerNatural = (playerValue == 21) && (playerHand.Count == 2);
                bool dealerNatural = (dealerValue == 21) && (playerHand.Count == 2);

                if (playerNatural && dealerNatural)
                {
                    FrontEnd.Output("Both have blackjack");

                    if (playerBet != null)
                    {
                        playerCredit += (double)playerBet;
                        playerBet = null;
                    }

                    return BlackjackResult.BothNatural;
                }
                else if (playerNatural)
                {
                    FrontEnd.Output("Player Blackjack");

                    if (playerBet != null)
                    {
                        playerCredit += 2.5 * (double)playerBet;
                        playerBet = null;
                    }

                    return BlackjackResult.PlayerNatural;
                }
                else if (dealerNatural)
                {
                    FrontEnd.Output("Dealer Blackjack");

                    if (playerBet != null)
                    {
                        playerBet = null;
                    }

                    return BlackjackResult.DealerNatural;
                }
            }

            if (dealerValue == playerValue)
            {
                FrontEnd.Output("Stand off");
                
                //Keep playing with the same bet

                return BlackjackResult.StandOff;
            }
            if (dealerValue > playerValue)
            {
                FrontEnd.Output("Dealer wins");

                if (playerBet != null)
                {
                    playerBet = null;
                }

                return BlackjackResult.DealerWins;
            }

            FrontEnd.Output("Player wins");

            if (playerBet != null)
            {
                playerCredit += 2d * (double)playerBet;
                playerBet = null;
            }

            return BlackjackResult.PlayerWins;
        }

        public List<Card> LookAtDealerHand()
        {
            if (currentState == BlackjackState.PlayerTurn)
            {
                var temp = dealerHand;
                temp.Remove(temp[0]);
                return temp;
            }
            return dealerHand;
        }

        public int ShoeCount()
        {
            return cardShoe.Count;
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

        /// <summary>
        /// Returns a comma-separated string of card IDs from the given hand.
        /// </summary>
        /// <param name="hand">The list of Card objects representing the hand.</param>
        /// <returns>A string containing the IDs of the cards in the hand, separated by commas.</returns>
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
