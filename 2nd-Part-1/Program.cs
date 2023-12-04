using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2nd_Part_1
{
    struct Draw
    {
        public int R, G, B;
    }

    struct Game
    {
        public int ID;
        public Draw[] Draws;
    }

    class Program
    {
        static IEnumerable<Game> LoadGames()
        {
            var lines = File.ReadAllLines("input.txt");

            var games = lines.Select(x => ParseGame(x));

            return games;
        }

        private static Game ParseGame(string str)
        {
            var ret = new Game();
            var partSplits = str.Split(": ");

            ret.ID = int.Parse(partSplits[0].Replace("Game ", ""));

            var drawSplits = partSplits[1].Split("; ");

            ret.Draws = new Draw[drawSplits.Count()];

            for (var i = 0; i < drawSplits.Count(); i++)
            {
                ret.Draws[i] = ParseDraw(drawSplits[i]);
            }
            
            return ret;
        }

        private static Draw ParseDraw(string str)
        {
            var ret = new Draw();

            var colourSplits = str.Split(", ");

            foreach(var colourSplit in colourSplits)
            {
                var parts = colourSplit.Split(" ");

                int count = int.Parse(parts[0]);
                
                if(parts[1] == "red")
                {
                    ret.R = count;
                }
                else if(parts[1] == "green")
                {
                    ret.G = count;
                }
                else
                {
                    ret.B = count;
                }
            }

            return ret;
        }

        private static bool GameIsValid(Game game, Draw limit)
        {
            return game.Draws.All(x => DrawIsValid(x, limit));
        }

        private static bool DrawIsValid(Draw draw, Draw limit)
        {
            return draw.R <= limit.R && draw.G <= limit.G && draw.B <= limit.B;
        }

        static void Main(string[] args)
        {
            var games = LoadGames().ToArray();
            var limit = new Draw() { R = 12, G = 13, B = 14 };
            var validGames = games.Where(x => GameIsValid(x, limit));
            var answer = validGames.Select(x => x.ID).Sum();

            Console.Write(answer);
            Console.ReadKey();
        }


    }
}
