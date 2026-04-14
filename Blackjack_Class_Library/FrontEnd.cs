using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Class_Library
{
    public static class FrontEnd
    {
        public static bool writeToConsole = false;

        public static void Output(string str)
        {
            if (writeToConsole)
            {
                Console.WriteLine(str);
            }
        }
    }
}
