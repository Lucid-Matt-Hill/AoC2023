using System;
using System.IO;
using System.Linq;

namespace _1st_Part_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawLines = File.ReadAllLines("input.txt");
            var justNumberStrings = rawLines.Select(str => string.Concat(str.Where(Char.IsDigit)));
            var justFirstAndLastNumberStrings = justNumberStrings.Select(str => str.First().ToString() + str.Last().ToString());
            var numbers = justFirstAndLastNumberStrings.Select(str => int.Parse(str));
            var answer = numbers.Sum();

            Console.Write(answer);
            Console.ReadKey();
        }
    }
}
