using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day12)}")]
    public class Day12 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";

            var spl = _input.Split('\n');
            int[,] grid = new int[spl[0].Length, spl.Length];

            Position goal = new(0, 0);
            Position start = new(0, 0);

            for (int x = 0; x < spl[0].Length; x++)
            {
                for (int y = 0; y < spl.Length; y++)
                {
                    grid[x, y] = spl[y][x] == 'E' ? 'z' - 96 : spl[y][x] == 'S' ? 'a' - 96 : spl[y][x] - 96;
                    if (spl[y][x] == 'E')
                    {
                        goal = new(x, y);
                    }
                    else if (spl[y][x] == 'S')
                    {
                        start = new(x, y);
                    }
                    var p = new Position(x, y);
                }
            }

            //List<Node> pQ = new(); // list of open nodes
            //pQ.Add(nodes[start]); // add starting node
            //List<Node> closed = new();
            //bool done = false;

            Expression<Func<Position, Position, bool>> ffx = (s, dest) => grid[s.X, s.Y] >= grid[dest.X, dest.Y] || grid[s.X, s.Y] + 1 == grid[dest.X, dest.Y];
            var ff = ffx.Compile();

            var star = PathFinding.AStarSearch(start, goal, (p) => ValidDests(p, grid, ff), Distance, (s, d) => 1);

            int min = int.MaxValue;

            //for (int x = 0; x < spl[0].Length; x++)
            //{
            //    for (int y = 0; y < spl.Length; y++)
            //    {
            //        if (grid[x, y] == 1)
            //        {
            //            start = new Position(x, y);
            //            var c = PathFinding.BreadthFirstSearch(start, goal, (p) => ValidDests(p, grid, ff));

            //            if (c.Any() && c.FirstOrDefault()?.X == start.X && c.FirstOrDefault()?.Y == start.Y)
            //            {
            //                if (c.Count < min)
            //                {
            //                    min = c.Count;
            //                }
            //            }
            //        }
            //    }
            //}

            Console.WriteLine("DONE");

            var path = star;

            char[,] pathVis = new char[spl[0].Length, spl.Length];

            for (int x = 0; x < spl[0].Length; x++)
            {
                for (int y = 0; y < spl.Length; y++)
                {
                    var ps = path.Where(n => n.X == x && n.Y == y).ToList();

                    if (ps.Count == 1)
                    {
                        var i = path.IndexOf(ps[0]);
                        if (i != path.Count - 1)
                        {
                            pathVis[x, y] = DirChar(ps[0], path[i + 1]);
                        }
                        else
                        {
                            pathVis[x, y] = 'E';
                        }
                    }
                    else if (ps.Count > 1)
                    {
                        Console.WriteLine($"Error {x}, {y}");
                    }
                    else
                    {
                        pathVis[x, y] = '.';
                    }
                }
            }


            _result += $"\npart 1 sum: {path.Count - 1}";

            _result += $"\n{StringifyGrid(pathVis)}";

            path = PathFinding.BreadthFirstSearch(start, goal, (p) => ValidDests(p, grid, ff));

            pathVis = new char[spl[0].Length, spl.Length];

            for (int x = 0; x < spl[0].Length; x++)
            {
                for (int y = 0; y < spl.Length; y++)
                {
                    var ps = path.Where(n => n.X == x && n.Y == y).ToList();

                    if (ps.Count == 1)
                    {
                        var i = path.IndexOf(ps[0]);
                        if (i != path.Count - 1)
                        {
                            pathVis[x, y] = DirChar(ps[0], path[i + 1]);
                        }
                        else
                        {
                            pathVis[x, y] = 'E';
                        }
                    }
                    else if (ps.Count > 1)
                    {
                        Console.WriteLine($"Error {x}, {y}");
                    }
                    else
                    {
                        pathVis[x, y] = '.';
                    }
                }
            }

            _result += $"\n\n{StringifyGrid(pathVis)}";
            _result += $"\npart 1 sum: {path.Count - 1}";


            _result += $"\npart 2: {min - 1}";
        }




        public char DirChar(Position cur, Position n)
        {
            if (cur.Y < n.Y)
            {
                return 'v';
            }

            if (cur.Y > n.Y)
            {
                return '^';
            }

            if (cur.X < n.X)
            {
                return '>';
            }

            if (cur.X > n.X)
            {
                return '<';
            }
            return 'x';
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

        private static IEnumerable<Position> Surrounding(Position pos) =>
            new[]
            {
                pos with {X = pos.X + 1},
                pos with {X = pos.X - 1},
                pos with {Y = pos.Y + 1},
                pos with {Y = pos.Y - 1}
            };

        private bool InBounds(Position p, int[,] grid)
        {
            return p.X >= 0 && p.Y >= 0 && p.X <= grid.GetUpperBound(0) && p.Y <= grid.GetUpperBound(1);
        }

        public List<Position> ValidDests(Position p, int[,] grid, Func<Position, Position, bool> isValid)
        {
            List<Position> res = new();

            foreach (var item in Surrounding(p))
            {
                if (InBounds(item, grid) && isValid.Invoke(p, item))
                {
                    res.Add(item);
                }
            }

            return res;
        }

        public static int Distance(Position s, Position d)
        {
            //return Math.Abs(s.Y - d.Y) + Math.Abs(s.X - d.X);
            return (int)Math.Round(Math.Sqrt(Math.Pow(s.X - d.X, 2) + Math.Pow(s.Y - d.Y, 2)));
        }
    }



    public sealed record class Position : IEquatable<Position>, PathFinding.IPathable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position? Parent { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public bool Equals(Position? other)
        {
            return other != null && X == other.X && Y == other.Y;
        }
    }


}
