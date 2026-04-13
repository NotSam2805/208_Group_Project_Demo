// See https://aka.ms/new-console-template for more information
using _208_Group_Project_Demo;

///<summary>
///Plays blackjack, until user ends.
///Displays the running count and true count on every player turn
///</summary>

var game = new Blackjack();
var counter = new CardCounter(game);

bool playing = true;
string input;
while (playing)
{
    Console.WriteLine("\n");

    Console.WriteLine($"Running count: {counter.GetRunningCount()}\nTrue count: {counter.GetTrueCount()}\n");

    game.ShowPlayerCredit();
    Console.WriteLine("Place a bet? (y/n)");
    input = Console.ReadLine();
    if (input == "y")
    {
        Console.WriteLine("Bet amount:");
        input = Console.ReadLine();
        game.PlaceBet(Convert.ToDouble(input));
    }
    Console.WriteLine();

    game.StartBlackjack();

    while(game.currentState == BlackjackState.PlayerTurn)
    {
        Console.WriteLine();
        if (game.CanDoubleDown())
        {
            Console.WriteLine("Hit or Stand or Double down? (H/S/D)");
        }
        else
        {
            Console.WriteLine("Hit or Stand? (H/S)");
        }
        input = Console.ReadLine();

        if (input == "H" || input == "h")
        {
            game.PlayerTurn(BlackjackAction.Hit);
        }
        else if (input == "D" || input == "d")
        {
            game.PlayerTurn(BlackjackAction.DoubleDown);
            game.ShowPlayerCredit();
        }
        else
        {
            game.PlayerTurn(BlackjackAction.Stand);
        }
    }

    while(game.currentState == BlackjackState.DealerTurn) { game.DealerTurn(); }

    game.EndGame();

    Console.WriteLine();
    
    game.ShowPlayerCredit();

    Console.WriteLine();

    Console.WriteLine("Keep playing? (y/n)");
    input = Console.ReadLine();
    if (input == "n")
    {
        playing = false;
    }
}