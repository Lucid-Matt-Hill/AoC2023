using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _6th_Part_1
{
    struct Race
    {
        public int Duration;
        public int RecordDistance;
    }
    class Program
    {
        static void Main(string[] args)
        {
            var races = LoadRaces("input.txt");
            var winningButtonDurations = races.Select(CalculateWinningButtonDurations).ToArray();
            var totalWaysToWin = winningButtonDurations.Select(x => x.Count());

            var answer = totalWaysToWin.Aggregate(1, (x, y) => x * y);
            Console.Write(answer);
            Console.ReadKey();
        }

        private static IEnumerable<int> CalculateWinningButtonDurations(Race race)
        {
            var times = Enumerable.Range(0, race.Duration);
            return times.Where(x => CalculateDistanceForTimeHeld(x, race.Duration) > race.RecordDistance);
        }

        private static int CalculateDistanceForTimeHeld(int timeHeld, int raceDuration)
        {
            return (timeHeld * (raceDuration - timeHeld));
        }

        private static IEnumerable<int> ParseNumbers(string line)
        {
            return line.Split(":")[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse);
        }

        private static IEnumerable<Race> LoadRaces(string filename)
        {
            var lines = File.ReadLines(filename).ToArray();
            var times = ParseNumbers(lines[0]);
            var distances = ParseNumbers(lines[1]);
            return times.Zip(distances, (a, b) => new Race() { Duration = a, RecordDistance = b });
        }
    }
}
