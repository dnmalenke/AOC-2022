using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Text;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day14)}")]
    public class Day14 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            int sum = 0;

            var spl = _input.Split('\n');
            List<Line> lines = new();
            foreach (var line in spl)
            {
                lines.Add(new(line));
            }

            int minX = lines.MinVal(l => l.MinX);
            int maxX = lines.MaxVal(l => l.MaxX) + 1;
            int maxY = lines.MaxVal(l => l.MaxY) + 1;

            int width = maxX - minX;

            char[,] scan = new char[width, maxY];

            scan.Fill('.');

            lines.ForEach(l => l.AddLines(ref scan, minX));

            while (!DropSand(ref scan, ref width, maxY, ref minX, true))
            {
                sum++;
            }

            _result += $"\npart 1: {sum}";
            _result += $"\n\n{Util.StringifyGrid(scan)}";

            sum = 0;

            lines.Add(new($"{minX},{maxY + 1} -> {maxX},{maxY + 1}"));

            minX = lines.MinVal(l => l.MinX);
            maxX = lines.MaxVal(l => l.MaxX) + 1;
            maxY = lines.MaxVal(l => l.MaxY) + 1;

            width = maxX - minX;

            scan = new char[width, maxY];

            scan.Fill('.');

            foreach (var line in lines)
            {
                line.AddLines(ref scan, minX);
            }

            while (!DropSand(ref scan, ref width, maxY, ref minX, false))
            {
                sum++;
            }

            _result += $"\npart 2: {sum + 1}";
            _result += $"\n\n{Util.StringifyGrid(scan)}";
        }

        private static bool DropSand(ref char[,] grid, ref int width, int height, ref int offset, bool p1)
        {
            int x = 500 - offset;
            int i = 0;
            List<Point> p = new();
            while (i < height)
            {
                if (i != 0)
                {
                    p.Add(new Point(x, i - 1));

                }

                if (grid[x, i] != '.')
                {

                    if (x - 1 == -1) // reached far left
                    {
                        if (p1)
                            return true;
                        width++;
                        ResizeArray(ref grid, width, height, true);

                        for (int yy = 0; yy < height - 1; yy++)
                        {
                            grid[x, yy] = '.';
                        }
                        grid[x, height - 1] = '#';
                        x++;
                        offset--;
                        //foreach (var pp in p)
                        //{
                        //    grid[pp.X, pp.Y] = '~';
                        //}

                        //grid[x, i - 1] = 'X';
                        //return true;
                    }

                    if (x + 1 == width)
                    {
                        if (p1)
                            return true;
                        width++;
                        ResizeArray(ref grid, width, height, false);

                        for (int yy = 0; yy < height - 1; yy++)
                        {
                            grid[x + 1, yy] = '.';
                        }
                        grid[x + 1, height - 1] = '#';
                    }

                    if (grid[x - 1, i] == '.') // left
                    {
                        x--;
                    }
                    else if (x + 1 != width && grid[x + 1, i] == '.') // right
                    {
                        x++;
                    }
                    else
                    {
                        break;
                    }
                }
                i++;
            }

            grid[x, i - 1] = 'O';

            if (i == 1)
            {
                return true;
            }

            return false;
        }

        private static void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum, bool left)
        {
            var newArray = new T[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, (co + (left ? 1 : 0)) * columnCount2, columnCount);
            original = newArray;
        }

        private class Line
        {
            public List<Point> Points { get; set; } = new();

            public Line(string l)
            {
                foreach (var item in l.Split("-"))
                {
                    Points.Add(new Point(item));
                }
            }

            public void AddLines(ref char[,] scan, int offset)
            {
                for (int i = 0; i < Points.Count - 1; i++)
                {
                    var p1 = Points[i];
                    var p2 = Points[i + 1];

                    if (p1.X == p2.X)
                    {
                        for (int y = p1.Y; p1.Y > p2.Y ? y >= p2.Y : y <= p2.Y; y += (p1.Y > p2.Y ? -1 : 1))
                        {
                            scan[p1.X - offset, y] = '#';
                        }
                    }
                    else if (p1.Y == p2.Y)
                    {
                        for (int x = p1.X; p1.X > p2.X ? x >= p2.X : x <= p2.X; x += (p1.X > p2.X ? -1 : 1))
                        {
                            //   Console.WriteLine($"set {x - offset},{p1.Y}");
                            scan[x - offset, p1.Y] = '#';
                        }
                    }
                }
            }

            public int MaxY => Points.MaxVal(x => x.Y);

            public int MaxX => Points.MaxVal(x => x.X);

            public int MinX => Points.MinVal(x => x.X);
        }
    }
}
