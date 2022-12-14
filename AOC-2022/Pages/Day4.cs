using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day4)}")]
    public class Day4 : DayTemplate
    {
        protected override void Run()
        {
            int sum = 0;

            _result = "";

            foreach (var line in _input.Lines)
            {
                string[] pairs = line.Split(',');
                Pair pair0 = new(pairs[0]);
                Pair pair1 = new(pairs[1]);
                if (ContainEachOther(pair0, pair1))
                {
                    sum++;
                }
            }

            _result += $"Part 1 sum: {sum}";

            sum = 0;

            foreach (var line in _input.Lines)
            {
                string[] pairs = line.Split(',');
                Pair pair0 = new(pairs[0]);
                Pair pair1 = new(pairs[1]);
                if (OverlapAtAll(pair0, pair1))
                {
                    sum++;
                }
            }

            _result += $"\nPart 2 sum: {sum}";

            StateHasChanged();
        }

        private static bool ContainEachOther(Pair pair0, Pair pair1)
        {
            Pair bigger = pair0.Length > pair1.Length ? pair0 : pair1;
            Pair smaller = pair0.Length > pair1.Length ? pair1 : pair0;

            return bigger.Range.Intersect(smaller.Range).ToList().Count == smaller.Length;
        }

        private static bool OverlapAtAll(Pair pair0, Pair pair1)
        {
            Pair bigger = pair0.Length > pair1.Length ? pair0 : pair1;
            Pair smaller = pair0.Length > pair1.Length ? pair1 : pair0;

            return bigger.Range.Intersect(smaller.Range).ToList().Count > 0;
        }

        private class Pair
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Length { get { return End - Start + 1; } }

            public IEnumerable<int> Range { get { return Enumerable.Range(Start, Length); } }

            public Pair(string input)
            {
                string[] spl = input.Split('-');
                Start = int.Parse(spl[0]);
                End = int.Parse(spl[1]);
            }
        }
    }
}
