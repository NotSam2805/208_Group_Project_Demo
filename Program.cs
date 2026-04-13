// See https://aka.ms/new-console-template for more information
using _208_Group_Project_Demo;

void PrintCard(Card card)
{
    Console.WriteLine(card.id);
}

void PrintDeck(Card[] deck) {
    for (int i = 0; i < deck.Length; i++)
    {
        PrintCard(deck[i]);
    }
}

var game = new Blackjack();

Console.ReadLine();