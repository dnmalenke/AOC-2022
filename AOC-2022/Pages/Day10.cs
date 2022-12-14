using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day10)}")]
    public class Day10 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            int cyc = 0;
            int sum = 0;

            Dictionary<string, int> map = new()
            {
                { "addx", 2 },
                { "noop", 1 }
            };

            List<int> track = new()
            {
                20,
                60,
                100,
                140,
                180,
                220
            };

            int x = 1;
            foreach (var l in _input.Split('\n'))
            {
                string op = l.Split(' ')[0];
                cyc += map[op];

                if (track.Contains(cyc))
                {
                    _result += $"\n cyc: {cyc}, cyc*x: {cyc * x}";
                    sum += cyc * x;
                }
                else if (op == "addx" && track.Contains(cyc - 1))
                {
                    _result += $"\n cyc: {cyc - 1}, cyc*x: {(cyc - 1) * x}";
                    sum += (cyc - 1) * x;
                }

                if (op == "addx")
                {
                    x += int.Parse(l.Split(' ')[1]);
                }

            }

            _result += $"\npart 1 sum: {sum}";
            x = 1;


            int li = 0;
            var spl = _input.Split("\n");

            char[,] crt = new char[40, 6];
            int posx = 0; int posy = 0;

            int subCyc = 0;

            for (int i = 1; i <= 240; i++)
            {
                crt[posx, posy] = Math.Abs(x - posx) <= 1 ? '#' : '.';
                posx++;
                if (posx == 40)
                {
                    posy++;
                    posx = 0;
                }

                if (Op(spl[li]) == "addx")
                {
                    subCyc++;

                    if(subCyc == 2)
                    {
                        x += int.Parse(spl[li].Split(' ')[1]);
                        subCyc = 0;
                        _result += $"\n x: {x}, parse{int.Parse(spl[li].Split(' ')[1])}";
                        li++;
                    }
                }
                else
                {
                    li++;
                }
            }

            _result += $"\n{Util.StringifyGrid(crt)}";
            _result += $"\npart 2: {cyc}";
        }

        private static string Op(string li)
        {
            return li.Split(' ')[0];
        }
    }
}
