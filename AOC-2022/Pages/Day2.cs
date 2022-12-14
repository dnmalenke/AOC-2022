using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day2)}")]
    public class Day2 : DayTemplate
    {
        protected override void Run()
        {
            int score = 0;

            foreach (var line in _input.Lines)
            {
                switch (line[2])
                {
                    case 'X':
                        score += 1;

                        score += Util.Case(line[0], ('A', 3), ('B', 0), ('C', 6));
                        break;
                    case 'Y':
                        score += 2;

                        score += Util.Case(line[0], ('A', 6), ('B', 3), ('C', 0));
                        break;
                    case 'Z':
                        score += 3;

                        score += Util.Case(line[0], ('A', 0), ('B', 6), ('C', 3));
                        break;
                }
            }

            _result = $"part 1 score: {score}";
            score = 0;

            foreach (var line in _input.Split("\n"))
            {
                switch (line[2])
                {
                    case 'X':
                        score += Util.Case(line[0], ('A', 3), ('B', 1), ('C', 2));
                        break;
                    case 'Y':
                        score += 3;

                        score += Util.Case(line[0], ('A', 1), ('B', 2), ('C', 3));
                        break;
                    case 'Z':
                        score += 6;

                        score += Util.Case(line[0], ('A', 2), ('B', 3), ('C', 1));
                        break;
                }
            }

            _result += $"\npart 2 score: {score}";

            StateHasChanged();
        }
    }
}
