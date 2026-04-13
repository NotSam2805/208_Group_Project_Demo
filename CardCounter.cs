using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _208_Group_Project_Demo
{
    public class CardCounter
    {
        public Blackjack game;
        private int runningCount;

        public CardCounter(Blackjack _game)
        {
            game = _game;
            runningCount = 0;
        }

        public int GetRunningCount()
        {
            if (game.currentState == BlackjackState.NoGame)
            {
                runningCount = 0;
            }

            return runningCount;
        }
    }
}
