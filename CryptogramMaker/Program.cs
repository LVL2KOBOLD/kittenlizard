using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            Console.Clear();

            Console.Write("Enter puzzle seed (or leave blank for random): ");
            string input = Console.ReadLine() ?? "";

            int seed;
            if (!int.TryParse(input, out seed))
                seed = new Random().Next();

            Random rng = new Random(seed);

            Console.WriteLine($"\nSeed locked: {seed}");

            string[] quotes = File.Exists("quotes.txt")
                ? File.ReadAllLines("quotes.txt")
                : Array.Empty<string>();

            string originalQuote = quotes[rng.Next(quotes.Length)].ToUpper();

            var key = GenerateCipherKey(rng);

            string cryptogram = Encrypt(originalQuote, key);

            Dictionary<char, char> playerMap = new Dictionary<char, char>();

            bool won = false;

            while (!won)
            {
                Console.Clear();

                Console.WriteLine("=== CRYPTOGRAM PUZZLE ===\n");
                Console.WriteLine(DisplayPuzzle(cryptogram, playerMap));

                if (CheckWin(originalQuote, cryptogram, playerMap))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nYOU SOLVED IT!");
                    Console.ResetColor();
                    won = true;
                    break;
                }

                Console.WriteLine("\nCommands:");
                Console.WriteLine("map A B");
                Console.WriteLine("reveal");
                Console.WriteLine("exit");

                Console.Write("\n> ");
                string cmd = Console.ReadLine() ?? "";
                cmd = cmd.Trim();

                if (cmd.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    return;

                if (cmd.Equals("reveal", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Console.WriteLine("=== SOLUTION ===\n");
                    Console.WriteLine(originalQuote);
                    Console.WriteLine("\nPress ENTER...");
                    Console.ReadLine();
                    continue;
                }

                if (cmd.StartsWith("map ", StringComparison.OrdinalIgnoreCase))
                {
                    string[] parts = cmd.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 3)
                    {
                        char cipherLetter = char.ToUpper(parts[1][0]);
                        char realLetter = char.ToUpper(parts[2][0]);

                        playerMap[cipherLetter] = realLetter;
                    }
                }
            }

            Console.WriteLine("\nPlay again? (y/n)");
            string again = Console.ReadLine()?.ToLower() ?? "n";
            playAgain = again == "y";
        }
    }

    static bool CheckWin(string original, string cipher, Dictionary<char, char> map)
    {
        for (int i = 0; i < cipher.Length; i++)
        {
            char c = cipher[i];

            if (!char.IsLetter(c))
                continue;

            if (!map.ContainsKey(c))
                return false;

            if (map[c] != original[i])
                return false;
        }

        return true;
    }

    static Dictionary<char, char> GenerateCipherKey(Random rng)
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] shuffled = alphabet.ToCharArray();

        do
        {
            for (int i = 0; i < shuffled.Length; i++)
            {
                int j = rng.Next(shuffled.Length);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }
        }
        while (HasSelfMapping(alphabet, shuffled));

        var key = new Dictionary<char, char>();

        for (int i = 0; i < alphabet.Length; i++)
            key[alphabet[i]] = shuffled[i];

        return key;
    }

    static bool HasSelfMapping(string original, char[] shuffled)
    {
        for (int i = 0; i < original.Length; i++)
            if (original[i] == shuffled[i])
                return true;

        return false;
    }

    static string Encrypt(string text, Dictionary<char, char> key)
    {
        var result = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
                result.Append(key[c]);
            else
                result.Append(c);
        }

        return result.ToString();
    }

    static string DisplayPuzzle(string cryptogram, Dictionary<char, char> playerMap)
    {
        var cipherLine = new StringBuilder();
        var guessLine = new StringBuilder();

        foreach (char c in cryptogram)
        {
            if (char.IsLetter(c))
            {
                cipherLine.Append(c);

                if (playerMap.ContainsKey(c))
                    guessLine.Append(playerMap[c]);
                else
                    guessLine.Append('_');
            }
            else
            {
                cipherLine.Append(c);
                guessLine.Append(' ');
            }
        }

        return cipherLine + "\n" + guessLine;
    }
}