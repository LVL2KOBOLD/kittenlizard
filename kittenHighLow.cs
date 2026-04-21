using System;

namespace HighLowGame
{
    class Program
    {
  static void Main(string[] args)
  {
      
      Console.WriteLine("Welcome to the High Low Game!");

      Console.WriteLine("Can i get your cat's name?");
      string userName = Console.ReadLine();

      Console.WriteLine("Welcome " + userName);

      Random random = new Random();
      int score = 0;
      int winning = 5;

        while (score < winning && score > -5)

      {

    int firstNumber = random.Next(1, 100);
    Console.WriteLine("First number is: " + firstNumber);

    Console.WriteLine("Is the next number Higher or Lower? (h/l) ");
    string guess = Console.ReadLine().ToLower();

    int secondNumber = random.Next(1, 100);
    Console.WriteLine("Second number is: " + secondNumber);

    if (secondNumber == firstNumber)
    {
        Console.WriteLine("Its a Match " + userName);
    }
    else if (guess == "h" && secondNumber > firstNumber)
    {
        Console.WriteLine("Correct! "+ userName);
        score++;
    }
    else if (guess == "l" && secondNumber < firstNumber)
    {
        Console.WriteLine("Correct! " + userName);
        score++;
    }
    else if (guess == "h" || guess == "l")
    {
        Console.WriteLine("Wrong! " + userName);
        score--;
    }
    else
    {
        Console.WriteLine("Sorry you must use (h or l)!");
    }
    
    Console.WriteLine("Score: " + score);    
      }
      if (score == winning)
      {
          Console.WriteLine("Winner: " + userName);
      }
      else if (score == -5)
      {
          Console.WriteLine("Dang, you Lost: " + userName);
      }
  }
    }

}
