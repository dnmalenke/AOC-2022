using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
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

            Position goal = new(0, 0);
            Position start = new(0, 0);

            int[,] grid = _input.ToProcessedGrid((x, y, val) =>
            {
                if (val == 'E')
                {
                    goal = new(x, y);
                    return 'z' - 96;
                }

                if (val == 'S')
                {
                    start = new(x, y);
                    return 'a' - 96;
                }

                return val - 96;
            });

            Expression<Func<Position, Position, bool>> ffx = (s, dest) => grid[s.X, s.Y] >= grid[dest.X, dest.Y] || grid[s.X, s.Y] + 1 == grid[dest.X, dest.Y];
            var ff = ffx.Compile();

            var star = PathFinding.AStarSearch(start, goal, (p) => ValidDests(p, grid, ff), Point.Distance, (s, d) => 1);

            int min = int.MaxValue;

            foreach ((int x, int y, object value) in grid.AllEnumerator())
            {
                if ((int)value == 1)
                {
                    start = new Position(x, y);
                    var c = PathFinding.BreadthFirstSearch(start, goal, (p) => ValidDests(p, grid, ff));

                    if (c.Any() && c.FirstOrDefault()?.X == start.X && c.FirstOrDefault()?.Y == start.Y)
                    {
                        if (c.Count < min)
                        {
                            min = c.Count;
                        }
                    }
                }
            }

            Console.WriteLine("DONE");

            var path = star;

            char[,] pathVis = new char[spl[0].Length, spl.Length];

            foreach ((int x, int y, object value) in pathVis.AllEnumerator())
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

            _result += $"\npart 1 sum: {path.Count - 1}";

            _result += $"\n{Util.StringifyGrid(pathVis)}";

            path = PathFinding.BreadthFirstSearch(start, goal, (p) => ValidDests(p, grid, ff));

            pathVis = new char[spl[0].Length, spl.Length];

            foreach ((int x, int y, object value) in pathVis.AllEnumerator())
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

            _result += $"\n\n{Util.StringifyGrid(pathVis)}";

            _result += $"\npart 2: {min - 1}";
        }

        public static char DirChar(Position cur, Position n) => cur.Y < n.Y ? 'v' : cur.Y > n.Y ? '^' : cur.X < n.X ? '>' : cur.X > n.X ? '<' : 'X';

        private static IEnumerable<Position> Surrounding(Position pos) =>
            new[]
            {
                pos with {X = pos.X + 1},
                pos with {X = pos.X - 1},
                pos with {Y = pos.Y + 1},
                pos with {Y = pos.Y - 1}
            };

        public static List<Position> ValidDests(Position p, int[,] grid, Func<Position, Position, bool> isValid)
        {
            List<Position> res = new();

            foreach (var item in Surrounding(p))
            {
                if (Util.InBounds(item.X, item.Y, grid) && isValid.Invoke(p, item))
                {
                    res.Add(item);
                }
            }

            return res;
        }
    }

    public sealed record class Position : Point, IEquatable<Position>, PathFinding.IPathable<Position>
    {
        public Position? Parent { get; set; }

        public Position(int x, int y) : base(x, y)
        {

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
