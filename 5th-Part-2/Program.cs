using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _5th_Part_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = LoadData("input.txt");

            var values = data.SeedRanges.ToArray();

            foreach (var map in data.Maps)
            {
                values = values.SelectMany(x => RemapRange(x, map)).ToArray();
            }

            var answer = values.Select(x => x.Start).Min();
            Console.Write(answer);
            Console.ReadKey();
        }

        struct MapEntry
        {
            public Range Source;
            public Range Dest;
        }

        struct Map
        {
            public List<MapEntry> Entries;
        }

        struct Range
        {
            public long Start;
            public long End;
        }

        struct Data
        {
            public IEnumerable<Range> SeedRanges;
            public IEnumerable<Map> Maps;
        }

        private static IEnumerable<Range> RemapRange(Range range, Map map)
        {
            var ret = new List<Range>();

            // Get re-mapped ranges
            foreach(var entry in map.Entries)
            {
                var remapRange = GetRangeOverlap(range, entry.Source);

                if(remapRange.HasValue)
                {
                    var shiftValue = entry.Dest.Start - entry.Source.Start;
                    ret.Add(new Range()
                    {
                        Start = remapRange.Value.Start + shiftValue,
                        End = remapRange.Value.End + shiftValue,
                    });
                }
            }

            // Get ranges between
            for (var i = 0; i < map.Entries.Count - 1; i++)
            {
                var gapRange = new Range()
                {
                    Start = map.Entries[i].Source.End + 1,
                    End = map.Entries[i + 1].Source.Start - 1,
                };

                var remapRange = GetRangeOverlap(range, gapRange);

                if (remapRange.HasValue)
                {
                    ret.Add(remapRange.Value);
                }
            }

            // Get start overhang
            if(range.Start < map.Entries.First().Source.Start)
            {
                ret.Add(new Range()
                {
                    Start = range.Start,
                    End = Math.Min(map.Entries.First().Source.Start - 1, range.End)
                });
            }

            // Get end overhang
            if (range.End > map.Entries.Last().Source.End)
            {
                ret.Add(new Range()
                {
                    Start = Math.Max(map.Entries.Last().Source.End + 1, range.Start),
                    End = range.End
                });
            }


            return ret;
        }

        private static Nullable<Range> GetRangeOverlap(Range a, Range b)
        {
            if(a.Start > b.End || b.Start > a.End)
            {
                return null;
            }

            return new Range()
            {
                Start = Math.Max(a.Start, b.Start),
                End = Math.Min(a.End, b.End)
            };
        }

        private static IEnumerable<Map> LoadMaps(string[] mapLines)
        {
            var ret = new List<Map>();

            mapLines = mapLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            foreach (var mapLine in mapLines)
            {
                if (mapLine.EndsWith(":"))
                {
                    ret.Add(new Map() { Entries = new List<MapEntry>() });
                }
                else
                {
                    var map = ret.Last();
                    map.Entries.Add(ParseMapEntry(mapLine));
                }
            }

            ret = ret.Select(SortRanges).ToList();

            return ret;
        }

        private static Map SortRanges(Map map)
        {
            map.Entries = map.Entries.OrderBy(x => x.Source.Start).ToList();
            return map;
        }

        private static MapEntry ParseMapEntry(string mapLine)
        {
            var numbers = mapLine.Split(" ").Select(long.Parse).ToArray();
            return new MapEntry()
            {
                Source = new Range() { Start = numbers[1], End = numbers[1] + numbers[2] - 1 },
                Dest = new Range() { Start = numbers[0], End = numbers[0] + numbers[2] - 1 }
            };
        }

        private static IEnumerable<Range> LoadSeedRanges(string[] lines)
        {
            var ret = new List<Range>();

            var numbers = lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToArray();

            for(int i = 0; i < numbers.Length; i += 2)
            {
                 ret.Add(new Range() { Start = numbers[i], End = numbers[i] + numbers[i + 1] - 1 });
            }

            return ret;
        }

        private static Data LoadData(string str)
        {
            var ret = new Data();

            var lines = File.ReadAllLines(str);

            ret.SeedRanges = LoadSeedRanges(lines); 

            ret.Maps = LoadMaps(lines.Skip(2).Take(lines.Length - 2).ToArray());

            return ret;
        }
    }
}
