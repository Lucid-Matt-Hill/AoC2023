using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _3rd_Part_1
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

        struct Symbol
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
                else if(!isDigit && beingParsed != null)
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
            for(int i = 0; i < lines.Length; i++)
            {
                ret = ret.Concat(ParseAllNumbers(lines[i], i));
            }
            return ret;
        }

        static IEnumerable<Symbol> ParseAllSymbols(string line, int lineIndex)
        {
            var ret = new List<Symbol>();

            for (int i = 0; i < line.Length; i++)
            {
                if(line[i] != '.' && !Char.IsDigit(line[i]))
                {
                    ret.Add(new Symbol()
                    {
                        LineIndex = lineIndex,
                        Index = i
                    });
                }
            }

            return ret;
        }

        static IEnumerable<Symbol> ParseAllSymbols(string[] lines)
        {
            var ret = Enumerable.Empty<Symbol>();
            for (int i = 0; i < lines.Length; i++)
            {
                ret = ret.Concat(ParseAllSymbols(lines[i], i));
            }
            return ret;
        }

        private static bool NumberContactsSymbol(Number number, Symbol symbol)
        {
            if(Math.Abs(number.LineIndex - symbol.LineIndex) > 1)
            {
                return false;
            }

            return symbol.Index >= (number.StartIndex - 1) && symbol.Index <= (number.EndIndex + 1);
        }

        private static bool NumberContactsAnySymbol(Number number, List<Symbol> symbols)
        {
            return symbols.Where(x => NumberContactsSymbol(number, x)).Any();
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var numbers = ParseAllNumbers(lines);
            var symbols = ParseAllSymbols(lines).ToList();

            var contactingNumbers = numbers.Where(x => NumberContactsAnySymbol(x, symbols));

            var answer = contactingNumbers.Select(x => x.Value).Sum();

            Console.Write(answer);
            Console.ReadKey();
        }
    }
}
