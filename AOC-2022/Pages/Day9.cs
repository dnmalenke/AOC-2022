using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day9)}")]
    public class Day9 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            Point headPos = new(0, 0);
            Point tailPos = new(0, 0);

            HashSet<Point> points = new()
            {
                new Point(tailPos.X, tailPos.Y)
            };

            foreach (var l in _input.Lines)
            {
                int c = l.ParseNumber();

                Point prevPos = new(headPos.X, headPos.Y);

                switch (l[0])
                {
                    case 'U':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.Y++;

                            if (TooFar(headPos, tailPos))
                            {
                                tailPos.X = prevPos.X;
                                tailPos.Y = prevPos.Y;
                                points.Add(new Point(tailPos.X, tailPos.Y));
                            }

                            prevPos = new Point(headPos.X, headPos.Y);
                        }
                        break;
                    case 'D':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.Y--;

                            if (TooFar(headPos, tailPos))
                            {
                                tailPos.X = prevPos.X;
                                tailPos.Y = prevPos.Y;
                                points.Add(new Point(tailPos.X, tailPos.Y));
                            }

                            prevPos = new Point(headPos.X, headPos.Y);
                        }
                        break;
                    case 'L':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.X--;

                            if (TooFar(headPos, tailPos))
                            {
                                tailPos.X = prevPos.X;
                                tailPos.Y = prevPos.Y;
                                points.Add(new Point(tailPos.X, tailPos.Y));
                            }

                            prevPos = new Point(headPos.X, headPos.Y);
                        }
                        break;
                    case 'R':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.X++;

                            if (TooFar(headPos, tailPos))
                            {
                                tailPos.X = prevPos.X;
                                tailPos.Y = prevPos.Y;
                                points.Add(new Point(tailPos.X, tailPos.Y));
                            }

                            prevPos = new Point(headPos.X, headPos.Y);
                        }
                        break;
                }
            }

            int sum = points.Count;
            _result += $"\npart 1 sum: {sum}";

            List<Point> queue = new();

            for (int i = 0; i < 10; i++)
            {
                queue.Add(new(0, 0));
            }

            headPos = queue[0];


            points = new()
            {
                new Point(queue[9].X, queue[9].Y)
            };

            HashSet<Point> points2 = new()
            {
                new Point(queue[0].X, queue[0].Y)
            };

            foreach (var l in _input.Lines)
            {
                int c = l.ParseNumber();
                // _result += $"\n{l}";
                switch (l[0])
                {
                    case 'U':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.Y++;

                            for (int j = 0; j < 9; j++)
                            {
                                if (TooFar(queue[j], queue[j + 1]))
                                {
                                    Move(queue[j], queue[j + 1]);

                                    if (j == 8)
                                    {
                                        points.Add(new Point(queue[9].X, queue[9].Y));
                                    }
                                }
                            }

                            points2.Add(new Point(queue[0].X, queue[0].Y));
                            //  Print2(queue);
                        }
                        break;
                    case 'D':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.Y--;

                            for (int j = 0; j < 9; j++)
                            {
                                if (TooFar(queue[j], queue[j + 1]))
                                {
                                    Move(queue[j], queue[j + 1]);

                                    if (j == 8)
                                    {
                                        points.Add(new Point(queue[9].X, queue[9].Y));
                                    }
                                }
                            }
                            points2.Add(new Point(queue[0].X, queue[0].Y));
                            //   Print2(queue);
                        }
                        break;
                    case 'L':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.X--;

                            for (int j = 0; j < 9; j++)
                            {
                                // Print2(queue);
                                if (TooFar(queue[j], queue[j + 1]))
                                {
                                    Move(queue[j], queue[j + 1]);

                                    if (j == 8)
                                    {
                                        points.Add(new Point(queue[9].X, queue[9].Y));
                                    }
                                }
                            }
                            points2.Add(new Point(queue[0].X, queue[0].Y));
                            //  Print2(queue);
                        }
                        break;
                    case 'R':
                        for (int i = 0; i < c; i++)
                        {
                            headPos.X++;

                            for (int j = 0; j < 9; j++)
                            {
                                if (TooFar(queue[j], queue[j + 1]))
                                {
                                    Move(queue[j], queue[j + 1]);

                                    if (j == 8)
                                    {
                                        points.Add(new Point(queue[9].X, queue[9].Y));
                                    }
                                }
                            }
                            points2.Add(new Point(queue[0].X, queue[0].Y));
                            //  Print2(queue);
                        }
                        break;
                }
            }

            sum = points.Count;

            _result += $"\npart 2: {sum}";
            _result += $"\n{Point.StringifyList(queue, p => queue.IndexOf(p).ToString()[0], reverse: true)}";

            //for (int i = points.MaxBy(p => p.Y).Y; i >= points.MinBy(p => p.Y).Y; i--)
            //{
            //    _result += "\n";
            //    for (int j = points.MinBy(p => p.X).X; j <= points.MaxBy(p => p.X).X; j++)
            //    {
            //        if (points.FirstOrDefault(p => p.X == j && p.Y == i) != null)
            //        {
            //            _result += "#";
            //        }
            //        else
            //        {
            //            _result += ".";
            //        }
            //    }
            //}

            //_result += "\n";
            //_result += "\n";
            //_result += "\n";

            //for (int i = points2.MaxBy(p => p.Y).Y; i >= points2.MinBy(p => p.Y).Y; i--)
            //{
            //    _result += "\n";
            //    for (int j = points2.MinBy(p => p.X).X; j <= points2.MaxBy(p => p.X).X; j++)
            //    {
            //        if (points2.FirstOrDefault(p => p.X == j && p.Y == i) != null)
            //        {
            //            _result += "#";
            //        }
            //        else
            //        {
            //            _result += ".";
            //        }
            //    }
            //}

        }

        private static void Move(Point dest, Point src)
        {
            if (dest.X != src.X)
            {
                if (dest.X > src.X)
                {
                    src.X++;
                }
                else
                {
                    src.X--;
                }
            }

            if (dest.Y != src.Y)
            {
                if (dest.Y > src.Y)
                {
                    src.Y++;
                }
                else
                {
                    src.Y--;
                }
            }
        }

        private void Print(List<Point> points2)
        {
            _result += "\n";
            for (int i = points2.MaxBy(p => p.Y)?.Y ?? 0; i >= points2.MinBy(p => p.Y)?.Y; i--)
            {
                _result += "\n";
                for (int j = points2.MinBy(p => p.X)?.X ?? 0; j <= points2.MaxBy(p => p.X)?.X; j++)
                {
                    if (points2.FirstOrDefault(p => p.X == j && p.Y == i) != null)
                    {
                        _result += points2.IndexOf(points2.FirstOrDefault(p => p.X == j && p.Y == i) ?? new(0, 0));
                    }
                    else
                    {
                        _result += ".";
                    }
                }
            }
        }

        private void Print2(List<Point> points2)
        {
            _result += "\n";
            for (int i = 20; i >= -20; i--)
            {
                _result += "\n";
                for (int j = -20; j <= 20; j++)
                {
                    if (points2.FirstOrDefault(p => p.X == j && p.Y == i) != null)
                    {
                        _result += points2.IndexOf(points2.FirstOrDefault(p => p.X == j && p.Y == i) ?? new(0, 0));
                    }
                    else
                    {
                        _result += ".";
                    }
                }
            }
        }

        private static bool TooFar(Point h, Point t)
        {
            return Math.Abs(h.X - t.X) > 1 || Math.Abs(h.Y - t.Y) > 1;
        }
    }
}
