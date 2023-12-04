using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _4th_Part_1
{
    struct Card
    {
        public IEnumerable<int> OwnNumbers;
        public IEnumerable<int> WinningNumbers;
    }

    class Program
    {
        private static IEnumerable<int> ParseNumberString(string str)
        {
            return str.Split(" ").Select(x => int.Parse(x));
        }
        private static Card ParseCard(string line)
        {
            var numberStrings = line.Replace("  ", " ").Split(": ")[1].Split(" | ");

            return new Card()
            {
                OwnNumbers = ParseNumberString(numberStrings[0]),
                WinningNumbers = ParseNumberString(numberStrings[1]),
            };
        }

        private static IEnumerable<int> FindCardsWinningNumbers(Card card)
        {
            return card.OwnNumbers.Where(card.WinningNumbers.Contains);
        }


        private static int CalculateCardScore(IEnumerable<int> cardWinningNumbers)
        {
            return (int)Math.Pow(2, cardWinningNumbers.Count() - 1);
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var cards = lines.Select(ParseCard).ToArray();
            var winningNumbers = cards.Select(FindCardsWinningNumbers);
            var cardScores = winningNumbers.Select(CalculateCardScore);
            var answer = cardScores.Sum();

            Console.Write(answer);
            Console.ReadKey();
        }

    }
}
