using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day5)}")]
    public class Day5 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";

            Dictionary<int, List<char>> supply = new();

            foreach (var line in _input.Lines)
            {
                if (line.StartsWith("move"))
                {
                    string[] insts = line.Split(' ');
                    int count = int.Parse(insts[1]);
                    int source = int.Parse(insts[3]);
                    int dest = int.Parse(insts[5]);

                    supply[dest].InsertRange(0, supply[source].Take(count).Reverse());
                    supply[source].RemoveRange(0, count);
                }
                else if (line != "")
                {
                    if (supply.Count == 0)
                    {
                        for (int i = 1; i <= (line.Length + 1) / 4; i++)
                        {
                            supply.Add(i, new());
                        }
                    }

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '[')
                        {
                            supply[i / 4 + 1].Add(line[i + 1]);
                        }
                    }
                }
            }

            string res = "";

            for (int i = 1; i <= supply.Count; i++)
            {
                res += supply[i][0];
            }

            _result += $"Part 1 res: {res}\n";

            supply = new();

            foreach (var line in _input.Lines)
            {
                if (line.StartsWith("move"))
                {
                    string[] insts = line.Split(' ');
                    int count = int.Parse(insts[1]);
                    int source = int.Parse(insts[3]);
                    int dest = int.Parse(insts[5]);

                    supply[dest].InsertRange(0, supply[source].Take(count));
                    supply[source].RemoveRange(0, count);
                }
                else if (line != "")
                {
                    if (supply.Count == 0)
                    {
                        for (int i = 1; i <= (line.Length + 1) / 4; i++)
                        {
                            supply.Add(i, new());
                        }
                    }

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '[')
                        {
                            supply[i / 4 + 1].Add(line[i + 1]);
                        }
                    }
                }
            }

            res = "";

            for (int i = 1; i <= supply.Count; i++)
            {
                res += supply[i][0];
            }

            _result += $"Part 2 res: {res}";

            StateHasChanged();
        }
    }
}