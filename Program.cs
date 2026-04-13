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
    game.StartBlackjack();

    while(game.currentState == BlackjackState.PlayerTurn)
    {
        Console.WriteLine($"Running count: {counter.GetRunningCount()}\nTrue count: {counter.GetTrueCount()}");
        Console.WriteLine();
        Console.WriteLine("Hit or Stand? (H/S)");
        input = Console.ReadLine();

        if (input == "H" || input == "h")
        {
            game.PlayerTurn(BlackjackAction.Hit);
        }
        else
        {
            game.PlayerTurn(BlackjackAction.Stand);
        }
    }

    while(game.currentState == BlackjackState.DealerTurn) { game.DealerTurn(); }

    game.EndGame();

    Console.WriteLine();

    Console.WriteLine("Keep playing? (y/n)");
    input = Console.ReadLine();
    if (input == "n")
    {
        playing = false;
    }
}