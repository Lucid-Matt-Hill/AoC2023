using System;
using System.IO;
using System.Linq;

namespace _1st_Part_2
{
    class Program
    {
        private static readonly string[] WRITTEN_NUMBERS = 
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" 
        };

        private static int FindFirstNumber(string str)
        {
            var ret = str;


            var firstIndexesOfWords = WRITTEN_NUMBERS.Select(x => ret.IndexOf(x));
            var earliestIndexOfAWord = firstIndexesOfWords.Where(x => x > -1).Min();

            int number = Array.IndexOf(firstIndexesOfWords.ToArray(), earliestIndexOfAWord);

            if (number > 9)
            {
                number -= 9;
            }

            return number;
        }

        private static int FindLastNumber(string str)
        {
            var ret = str;


            var firstIndexesOfWords = WRITTEN_NUMBERS.Select(x => ret.LastIndexOf(x));
            var earliestIndexOfAWord = firstIndexesOfWords.Max();

            int number = Array.IndexOf(firstIndexesOfWords.ToArray(), earliestIndexOfAWord);

            if (number > 9)
            {
                number -= 9;
            }

            return number;
        }

        static void Main(string[] args)
        {
            var rawLines = File.ReadAllLines("input.txt");
            var numbers = rawLines.Select(str => (FindFirstNumber(str) * 10) + FindLastNumber(str));
            var answer = numbers.Sum();

            Console.Write(answer);
            Console.ReadKey();
        }
    }
}
