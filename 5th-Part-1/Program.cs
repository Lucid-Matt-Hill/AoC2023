using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _5th_Part_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = LoadData("input.txt");

            var values = data.Seeds.ToArray();

            foreach (var map in data.Maps)
            {
                values = values.Select(x => RemapValue(x, map)).ToArray();
            }

            var answer = values.Min();
            Console.Write(answer);
            Console.ReadKey();
        }

        private static long RemapValue(long value, Map map)
        {
            foreach (var mapEntry in map.Entries)
            {
                if(value >= mapEntry.SourceRangeStart && value <= mapEntry.SourceRangeStart + mapEntry.RangeLength)
                {
                    return value + (mapEntry.DestRangeStart - mapEntry.SourceRangeStart);
                }
            }

            return value;
        }

        struct MapEntry
        {
            public long SourceRangeStart;
            public long DestRangeStart;
            public long RangeLength;
        }

        struct Map
        {
            public List<MapEntry> Entries;
        }

        struct Data
        {
            public IEnumerable<long> Seeds;
            public IEnumerable<Map> Maps;
        }


        private static IEnumerable<Map> LoadMaps(string[] mapLines)
        {
            var ret = new List<Map>();

            mapLines = mapLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            foreach (var mapLine in mapLines)
            {
                if(mapLine.EndsWith(":"))
                {
                    ret.Add(new Map() { Entries = new List<MapEntry>() });
                }
                else
                {
                    var map = ret.Last();
                    map.Entries.Add(ParseMapEntry(mapLine));
                }
            }
            
            return ret;
        }

        private static MapEntry ParseMapEntry(string mapLine)
        {
            var numbers = mapLine.Split(" ").Select(long.Parse).ToArray();
            return new MapEntry()
            {
                DestRangeStart = numbers[0],
                SourceRangeStart = numbers[1],
                RangeLength = numbers[2],
            };
        }

        private static Data LoadData(string str)
        {
            var ret = new Data();

            var lines = File.ReadAllLines(str);

            ret.Seeds = lines[0].Split(": ")[1].Split(" ").Select(long.Parse);

            ret.Maps = LoadMaps(lines.Skip(2).Take(lines.Length - 2).ToArray());

            return ret;
        }
    }
}
