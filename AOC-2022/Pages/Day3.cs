using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day3)}")]
    public class Day3 : DayTemplate
    {
        protected override void Run()
        {
            int sum = 0;

            _result = "";

            foreach (var line in _input.Lines)
            {
                string comp1 = line[..(line.Length / 2)];
                string comp2 = line[(line.Length / 2)..];
                foreach (var item in comp1.Intersect(comp2))
                {
                    if (char.IsLower(item))
                    {
                        sum += item - '`';
                    }
                    else
                    {
                        sum += item - 38;
                    }
                }
            }

            _result += $"\npart 1 sum: {sum}";

            sum = 0;

            foreach (var line in _input.Lines.Chunk(3))
            {
                foreach (var item in line[0].Intersect(line[1]).Intersect(line[2]))
                {
                    sum += item - (char.IsLower(item) ? '`' : 38);
                }
            }

            _result += $"\npart 2 sum: {sum}";

            StateHasChanged();
        }
    }
}
