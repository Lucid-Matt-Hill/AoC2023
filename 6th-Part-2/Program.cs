using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _6th_Part_2
{
    struct Race
    {
        public long Duration;
        public long RecordDistance;
    }

    class Program
    {

        static double SolveQuadraticPlus(double a, double b, double c)
        {
            return (-b + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        }

        static double SolveQuadraticMinus(double a, double b, double c)
        {
            return (-b - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        }

        static void Main(string[] args)
        {
            var race = LoadRace("input.txt");
            var winningButtonDurations = CalculateWinningButtonDurations(race);

            // Translation to quadratic equation
            // x * (Duration - x) = Distance
            // 0 = x * (Duration - x) -1Distance
            // 0 = -1x^2 + DurationX -1Distance

            var testa = SolveQuadraticPlus(-1, race.Duration, -race.RecordDistance);
            var testb = SolveQuadraticMinus(-1, race.Duration, -race.RecordDistance);

            var answer = Math.Ceiling(testb) - Math.Ceiling(testa);

            Console.Write(answer);
            Console.ReadKey();
        }

        private static IEnumerable<long> CalculateWinningButtonDurations(Race race)
        {
            return Enumerable.Empty<long>();
            //var times = Enumerable.Range(0, race.Duration);
            //return times.Where(x => CalculateDistanceForTimeHeld(x, race.Duration) > race.RecordDistance);
        }

        private static long CalculateDistanceForTimeHeld(long timeHeld, long raceDuration)
        {
            return (timeHeld * (raceDuration - timeHeld));
        }

        private static long ParseNumber(string line)
        {

            var rawNumberString = line.Split(":")[1];
            var cleanNumberString = rawNumberString.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray();

            return long.Parse(cleanNumberString);
        }

        private static Race LoadRace(string filename)
        {
            var lines = File.ReadLines(filename).ToArray();
            return new Race()
            {
                Duration = ParseNumber(lines[0]),
                RecordDistance = ParseNumber(lines[1]),
            };
        }
    }
}
