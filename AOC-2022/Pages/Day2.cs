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
            foreach (var line in _input.Split("\n"))
            {
                switch (line[2])
                {
                    case 'X':
                        score += 1;

                        switch (line[0])
                        {
                            case 'A':
                                score += 3;
                                break;
                            case 'B':
                                score += 0;
                                break;
                            case 'C':
                                score += 6;
                                break;
                        }
                        break;
                    case 'Y':
                        score += 2;

                        switch (line[0])
                        {
                            case 'A':
                                score += 6;
                                break;
                            case 'B':
                                score += 3;
                                break;
                            case 'C':
                                score += 0;
                                break;
                        }
                        break;
                    case 'Z':
                        score += 3;

                        switch (line[0])
                        {
                            case 'A':
                                score += 0;
                                break;
                            case 'B':
                                score += 6;
                                break;
                            case 'C':
                                score += 3;
                                break;
                        }
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
                        switch (line[0])
                        {
                            case 'A':
                                score += 3;
                                break;
                            case 'B':
                                score += 1;
                                break;
                            case 'C':
                                score += 2;
                                break;
                        }
                        break;
                    case 'Y':
                        score += 3;

                        switch (line[0])
                        {
                            case 'A':
                                score += 1;
                                break;
                            case 'B':
                                score += 2;
                                break;
                            case 'C':
                                score += 3;
                                break;
                        }
                        break;
                    case 'Z':
                        score += 6;

                        switch (line[0])
                        {
                            case 'A':
                                score += 2;
                                break;
                            case 'B':
                                score += 3;
                                break;
                            case 'C':
                                score += 1;
                                break;
                        }
                        break;
                }
            }

            _result += $"\npart 2 score: {score}";


            StateHasChanged();
        }
    }
}
