using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day1)}")]
    public class Day1 : DayTemplate
    {
        protected override void Run()
        {
            List<int> sums = new();

            int sum = 0;
            foreach (var line in _input.Split("\n"))
            {
                if (int.TryParse(line, out int num))
                {
                    sum += num;
                }
                else
                {
                    sums.Add(sum);
                    sum = 0;
                }
            }
            sums.Add(sum);
            sums.Sort();
            _result = $"part 1 max sum: {sums.Max()}";
            _result += "\npart 2: top 3 amounts:\n";
            sum = 0;
            foreach (var item in sums.TakeLast(3))
            {
                _result += $"{item}\n";
                sum += item;
            }
            _result += $"top 3 sum: {sum}";

            StateHasChanged();
        }
    }
}
