using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day7)}")]
    public class Day7 : DayTemplate
    {
        protected override void Run()
        {
            _result += "\nhello";
        }
    }
}
