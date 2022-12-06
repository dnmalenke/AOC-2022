using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day6)}")]
    public class Day6 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";

            Queue<char> q = new();

            int i = 0;

            foreach (var c in _input)
            {
                List<char> d = q.Distinct().ToList();

                if (q.Count == 4 && d.Count() == 4)
                {
                    break;
                }

                q.Enqueue(c);
                if (q.Count > 4)
                {
                    q.Dequeue();
                }

                i++;
            }

            _result += $"Part 1 res: {i}\n";

            q = new();

            i = 0;

            foreach (var c in _input)
            {
                List<char> d = q.Distinct().ToList();

                if (q.Count == 14 && d.Count() == 14)
                {
                    break;
                }

                q.Enqueue(c);
                if (q.Count > 14)
                {
                    q.Dequeue();
                }

                i++;
            }

            _result += $"Part 2 res: {i}";

            StateHasChanged();
        }
    }
}