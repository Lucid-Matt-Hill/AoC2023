using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _4th_Part_2
{
    struct Card
    {
        public IEnumerable<int> OwnNumbers;
        public IEnumerable<int> WinningNumbers;
        public int LocalScore;
        public IEnumerable<Card> AwardedCards;
        public int RecursivelyAwardedCardCount;
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

            var ret = new Card()
            {
                OwnNumbers = ParseNumberString(numberStrings[0]),
                WinningNumbers = ParseNumberString(numberStrings[1]),
            };

            ret.LocalScore = ret.OwnNumbers.Where(ret.WinningNumbers.Contains).Count();

            return ret;
        }

        private static void GatherAwardedCardsForEachCard(Card[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].AwardedCards = Enumerable.Empty<Card>();

                var awardedCardStartIndex = i + 1;

                if(awardedCardStartIndex >= cards.Length)
                {
                    continue;
                }

                var awardedCardEndIndex = Math.Min(i + cards[i].LocalScore, cards.Length - 1);

                if (awardedCardEndIndex >= awardedCardStartIndex)
                {
                    cards[i].AwardedCards = cards.Skip(awardedCardStartIndex).Take((awardedCardEndIndex - awardedCardStartIndex) + 1);
                }
            }
        }

        private static void GatherRecursivelyAwardedCardCounts(Card[] cards)
        {
            for(var i = cards.Length - 1; i >= 0; i--)
            {
                cards[i].RecursivelyAwardedCardCount = cards[i].AwardedCards.Count() + cards[i].AwardedCards.Select(x => x.RecursivelyAwardedCardCount).Sum();
            }
        }


        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var cards = lines.Select(ParseCard).ToArray();
            GatherAwardedCardsForEachCard(cards);
            GatherRecursivelyAwardedCardCounts(cards);

            var answer = cards.Count() + cards.Select(x => x.RecursivelyAwardedCardCount).Sum();
            Console.Write(answer);
            Console.ReadKey();
        }
    }
}
