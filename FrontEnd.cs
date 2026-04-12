using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _208_Group_Project_Demo
{
    //Static class to act as the front end, all input and output will come through this class and it will eventually act as an interface to the GUI
    public static class FrontEnd
    {
        public static void Output(string message)
        {
            Console.WriteLine(message);
        }

        public static string Input()
        {
            return Console.ReadLine();
        }
    }
}
