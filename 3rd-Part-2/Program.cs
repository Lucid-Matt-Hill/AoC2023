using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _3rd_Part_2
{
    class Program
    {
        struct Number
        {
            public int LineIndex;
            public int Value;
            public int StartIndex;
            public int EndIndex;
        }

        struct PotentialGear

        {
            public int LineIndex;
            public int Index;
        }

        static IEnumerable<Number> ParseAllNumbers(string line, int lineIndex)
        {
            var ret = new List<Number>();
            Nullable<Number> beingParsed = null;

            for (int i = 0; i < line.Length; i++)
            {
                var isDigit = Char.IsDigit(line[i]);

                if (isDigit && beingParsed == null)
                {
                    beingParsed = new Number()
                    {
                        LineIndex = lineIndex,
                        StartIndex = i
                    };
                }
                else if (!isDigit && beingParsed != null)
                {
                    var current = beingParsed.Value;
                    current.EndIndex = i - 1;
                    current.Value = int.Parse(line.Substring(current.StartIndex, i - current.StartIndex));

                    ret.Add(current);
                    beingParsed = null;
                }
            }

            if (beingParsed != null)
            {
                var current = beingParsed.Value;
                current.EndIndex = line.Length - 1;
                current.Value = int.Parse(line.Substring(current.StartIndex, line.Length - current.StartIndex));

                ret.Add(current);
                beingParsed = null;
            }

            return ret;
        }

        static IEnumerable<Number> ParseAllNumbers(string[] lines)
        {
            var ret = Enumerable.Empty<Number>();
            for (int i = 0; i < lines.Length; i++)
            {
                ret = ret.Concat(ParseAllNumbers(lines[i], i));
            }
            return ret;
        }

        static IEnumerable<PotentialGear> ParseAllPotentialGears(string line, int lineIndex)
        {
            var ret = new List<PotentialGear>();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '*')
                {
                    ret.Add(new PotentialGear()
                    {
                        LineIndex = lineIndex,
                        Index = i
                    });
                }
            }

            return ret;
        }

        static IEnumerable<PotentialGear> ParseAllPotentialGears(string[] lines)
        {
            var ret = Enumerable.Empty<PotentialGear>();
            for (int i = 0; i < lines.Length; i++)
            {
                ret = ret.Concat(ParseAllPotentialGears(lines[i], i));
            }
            return ret;
        }

        private static bool NumberContactsPotentialGear(Number number, PotentialGear potentialGear)
        {
            if (Math.Abs(number.LineIndex - potentialGear.LineIndex) > 1)
            {
                return false;
            }

            return potentialGear.Index >= (number.StartIndex - 1) && potentialGear.Index <= (number.EndIndex + 1);
        }

        private static int CalculateGearValue(PotentialGear potentialGear, IEnumerable<Number> numbers)
        {
            var contactingNumbers = numbers.Where(x => NumberContactsPotentialGear(x, potentialGear));

            if(contactingNumbers.Count() != 2)
            {
                return 0;
            }

            return contactingNumbers.First().Value * contactingNumbers.Last().Value;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var numbers = ParseAllNumbers(lines);
            var gears = ParseAllPotentialGears(lines).ToList();

            var answer = gears.Select(x => CalculateGearValue(x, numbers)).Sum();

            Console.Write(answer);
            Console.ReadKey();
        }


    }
}
