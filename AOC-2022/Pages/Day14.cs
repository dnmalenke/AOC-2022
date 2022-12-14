using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;
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
            List<Line> lines = new List<Line>();
            foreach (var line in spl)
            {
                lines.Add(new(line));
            }

            int minX = lines.MinBy(l => l.MinX).MinX;
            int maxX = lines.MaxBy(l => l.MaxX).MaxX + 1;
            int maxY = lines.MaxBy(l => l.MaxY).MaxY + 1;

            int width = maxX - minX;

            char[,] scan = new char[width, maxY];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    scan[x, y] = '.';
                }
            }

            foreach (var line in lines)
            {
                line.AddLines(ref scan, width, maxY, minX);
            }

            while (!DropSand(ref scan, ref width, maxY, ref minX, true))
            {
                sum++;
            }

            _result += $"\npart 1: {sum }";
            _result += $"\n\n{StringifyGrid(scan)}";


            sum = 0;

            lines.Add(new($"{minX},{maxY + 1} -> {maxX},{maxY + 1}"));

            minX = lines.MinBy(l => l.MinX).MinX;
            maxX = lines.MaxBy(l => l.MaxX).MaxX + 1;
            maxY = lines.MaxBy(l => l.MaxY).MaxY + 1;

            width = maxX - minX;

            scan = new char[width, maxY];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    scan[x, y] = '.';
                }
            }

            foreach (var line in lines)
            {
                line.AddLines(ref scan, width, maxY, minX);
            }

            while (!DropSand(ref scan, ref width, maxY, ref minX, false))
            {
                sum++;
            }

            _result += $"\npart 2: {sum + 1}";
            _result += $"\n\n{StringifyGrid(scan)}";

        }

        public bool DropSand(ref char[,] grid, ref int width, int height, ref int offset, bool p1)
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

                        for (int yy = 0; yy < height-1; yy++)
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
                        if(p1)
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

            if(i == 1)
            {
                return true;
            }

            return false;
        }

        void ResizeArray(ref char[,] original, int newCoNum, int newRoNum, bool left)
        {
            var newArray = new char[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum ;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, (co+ (left ? 1 : 0)) * columnCount2 , columnCount);
            original = newArray;
        }

        public string StringifyGrid(char[,] grid)
        {
            StringBuilder sb = new();

            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    sb.Append(grid[x, y]);
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        private class Line
        {
            public List<Point> Points { get; set; } = new();

            public Line(string l)
            {
                foreach (var item in l.Split("-"))
                {
                    string p = item.Trim('>').Trim();
                    var spl = p.Split(',');
                    Points.Add(new(int.Parse(spl[0]), int.Parse(spl[1])));
                }
            }

            public void AddLines(ref char[,] scan, int width, int height, int offset)
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

            public int MaxY => Points.MaxBy(x => x.Y).Y;

            public int MaxX => Points.MaxBy(x => x.X).X;

            public int MinX => Points.MinBy(x => x.X).X;
        }

        private class Point : IEquatable<Point>
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }

            public override bool Equals(object? obj)
            {
                return obj != null &&
                X == ((Point)obj).X &&
                Y == ((Point)obj).Y;
            }

            public bool Equals(Point? other)
            {
                return other != null &&
                X == ((Point)other).X &&
                Y == ((Point)other).Y;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
    }


}
